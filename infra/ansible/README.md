# Ansible: provision the VM and deploy BookIt

Roles, all idempotent (second run = 0 changed):

| Role | Does |
|---|---|
| `base` | apt packages, unattended security upgrades, 2 GB swap, ufw (22/80/443 in, deny rest), fail2ban |
| `docker` | Docker Engine + compose plugin from Docker's apt repo, log rotation |
| `caddy` | Caddy from its apt repo, `Caddyfile` with automatic Let's Encrypt TLS for `app_domain` |
| `deploy` | `/opt/bookit/{docker-compose.yml,.env}`, `docker compose pull` + `up`, waits for `/health/ready` |

```bash
cp ../../.env.example .env.prod          # fill in; gitignored
terraform -chdir=../terraform output -raw ansible_inventory > inventory.ini
ansible-galaxy collection install -r requirements.yml
ansible-playbook site.yml                 # first time: everything
ansible-playbook site.yml --tags deploy -e app_image_tag=sha-abc1234   # rollout / rollback
```

Site is then at `https://<vm-ip>.sslip.io`. Set `app_domain` in `group_vars/bookit.yml` to use a real domain (point an A record at the static IP first).

Needs `ansible` (not just `ansible-core`) 2.16+ on the control machine. On Windows use WSL.
