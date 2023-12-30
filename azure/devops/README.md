# Azure Devops

## Notes

Steps to setup new CI/CD Linux self-hosted agent:
1. Install Dotnet SDK
2. Install NodeJS
3. Install Docker
4. Post installation
   + Perform steps: https://docs.docker.com/engine/install/linux-postinstall/
   + Create `docker-compose.sh` to wrap `docker compose` (Docker compose V2) 
5. Install Azure Devops agent by running scripts

If you have multiple subscriptions from different tenants, you need to connect them to your organisation to be able to access them in pipelines.
