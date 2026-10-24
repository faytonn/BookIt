# One VM, one static IP, a private VPC and two firewall rules. Nothing else.

resource "google_compute_network" "vpc" {
  name                    = "${var.name}-vpc"
  auto_create_subnetworks = false
}

resource "google_compute_subnetwork" "subnet" {
  name          = "${var.name}-subnet"
  network       = google_compute_network.vpc.id
  region        = var.region
  ip_cidr_range = "10.10.0.0/24"
}

# Reserved address: survives VM recreation, so DNS and the Ansible inventory stay valid.
resource "google_compute_address" "public_ip" {
  name   = "${var.name}-ip"
  region = var.region
}

# SSH only from the admin IP.
resource "google_compute_firewall" "ssh" {
  name    = "${var.name}-allow-ssh"
  network = google_compute_network.vpc.id

  allow {
    protocol = "tcp"
    ports    = ["22"]
  }

  source_ranges = [var.admin_cidr]
  target_tags   = ["${var.name}-vm"]
}

# Web from anywhere. Caddy terminates TLS on the VM (Ansible role).
resource "google_compute_firewall" "web" {
  name    = "${var.name}-allow-web"
  network = google_compute_network.vpc.id

  allow {
    protocol = "tcp"
    ports    = ["80", "443"]
  }

  source_ranges = ["0.0.0.0/0"]
  target_tags   = ["${var.name}-vm"]
}

# Dedicated service account with no IAM roles: the VM needs no GCP API access.
resource "google_service_account" "vm" {
  account_id   = "${var.name}-vm"
  display_name = "${var.name} VM"
}

resource "google_compute_instance" "vm" {
  name         = "${var.name}-vm"
  machine_type = var.machine_type
  zone         = var.zone
  tags         = ["${var.name}-vm"]

  boot_disk {
    initialize_params {
      image = var.image
      size  = var.disk_size_gb
      type  = "pd-balanced"
    }
  }

  network_interface {
    subnetwork = google_compute_subnetwork.subnet.id

    access_config {
      nat_ip = google_compute_address.public_ip.address
    }
  }

  metadata = {
    ssh-keys               = "${var.ssh_user}:${trimspace(file(var.ssh_public_key_path))}"
    block-project-ssh-keys = "TRUE"
  }

  service_account {
    email  = google_service_account.vm.email
    scopes = ["logging-write", "monitoring-write"]
  }

  shielded_instance_config {
    enable_secure_boot          = true
    enable_vtpm                 = true
    enable_integrity_monitoring = true
  }

  # Replace the VM instead of trying to resize it in place.
  allow_stopping_for_update = true

  labels = {
    app = var.name
  }
}
