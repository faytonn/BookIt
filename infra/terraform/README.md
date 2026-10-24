# Terraform: BookIt VM on GCP

Creates: VPC + subnet, static IP, firewall (22 from `admin_cidr` only; 80/443 from anywhere), a Debian 12 `e2-medium` VM with a no-role service account and Shielded VM on.

```bash
gcloud auth application-default login
gcloud services enable compute.googleapis.com --project <project>
cp terraform.tfvars.example terraform.tfvars   # fill in
terraform init
terraform plan
terraform apply
terraform output ansible_inventory > ../ansible/inventory.ini
```

Costs money while it runs (~EUR 30/month for e2-medium + disk + IP). `terraform destroy` when not demoing.
State is local (`terraform.tfstate`, gitignored); fine for one person, use a GCS backend for a team.
