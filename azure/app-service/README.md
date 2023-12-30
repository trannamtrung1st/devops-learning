# Azure App Service Learning

## Instructions
1. Follow: https://learn.microsoft.com/en-us/azure/app-service/deploy-local-git?tabs=cli
2. `git remote add deployment <url>`
3. (Optional) `az webapp config appsettings set --name <app-name> --resource-group <group-name> --settings DEPLOYMENT_BRANCH='main'`
4. `git push deployment dev:main`
5. Verify deployment result

## Notes
+ Access Kudu portal: `https://<app-name>.scm.azurewebsites.net/`
+ If Linux containers, view Docker log instead of log stream => by enabling App Service logs. Reference: https://aka.ms/linux-diagnostics