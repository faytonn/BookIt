output "public_ip" {
  description = "Static public IP of the VM."
  value       = google_compute_address.public_ip.address
}

output "ssh_command" {
  value = "ssh -i ~/.ssh/bookit ${var.ssh_user}@${google_compute_address.public_ip.address}"
}

output "ansible_inventory" {
  description = "Paste into infra/ansible/inventory.ini."
  value       = "[bookit]\n${google_compute_address.public_ip.address} ansible_user=${var.ssh_user} ansible_ssh_private_key_file=~/.ssh/bookit"
}
