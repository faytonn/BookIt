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

## CI deploy (IAP)

`deploy.yml` never SSHes over the public internet: it opens an IAP TCP tunnel to port 22 as the `bookit-deployer` service account (only `iap.tunnelResourceAccessor` + `compute.viewer`). Firewall rule `bookit-allow-iap-ssh` allows 22 from Google's IAP range `35.235.240.0/20` only.

```bash
gcloud iam service-accounts keys create deployer-key.json --iam-account="$(terraform output -raw deployer_service_account)"
gh secret set GCP_SA_KEY < deployer-key.json && rm deployer-key.json
gh secret set GCP_PROJECT_ID --body "$(terraform output -raw project_id 2>/dev/null || echo <project>)"
gh secret set VM_SSH_KEY < ~/.ssh/bookit
gh secret set PROD_ENV_FILE < ../ansible/.env.prod
```
