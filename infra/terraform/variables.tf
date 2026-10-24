variable "project_id" {
  description = "GCP project that hosts the VM."
  type        = string
}

variable "region" {
  description = "GCP region."
  type        = string
  default     = "europe-west3"
}

variable "zone" {
  description = "GCP zone inside the region."
  type        = string
  default     = "europe-west3-a"
}

variable "name" {
  description = "Prefix for every resource name."
  type        = string
  default     = "bookit"
}

variable "machine_type" {
  description = "VM size. e2-medium = 2 vCPU / 4 GB, enough for app + SQL Server + RabbitMQ."
  type        = string
  default     = "e2-medium"
}

variable "image" {
  description = "Boot image."
  type        = string
  default     = "debian-cloud/debian-12"
}

variable "disk_size_gb" {
  description = "Boot disk size. SQL Server image alone is ~1.5 GB."
  type        = number
  default     = 30
}

variable "admin_cidr" {
  description = "CIDR allowed to SSH (your public IP, /32). Nothing else can reach port 22."
  type        = string

  validation {
    condition     = can(cidrnetmask(var.admin_cidr))
    error_message = "admin_cidr must be a CIDR like 203.0.113.4/32."
  }
}

variable "ssh_user" {
  description = "Login user created on the VM via instance metadata; Ansible connects as this user."
  type        = string
  default     = "deploy"
}

variable "ssh_public_key_path" {
  description = "Path to the public key for ssh_user."
  type        = string
}
