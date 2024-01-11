#!/bin/bash

# Replace empty or unset environment variables with default values
WebApiUrl="${WebApiUrl}"
PublicApiEndpoint="${PublicApiEndpoint}"

# Use sed to replace the values in appsettings.json
sed -i "s#\"WebApiUrl\": \"[^\"]*\"#\"WebApiUrl\": \"$WebApiUrl\"#g" appsettings.json
sed -i "s#\"PublicApiEndpoint\": \"[^\"]*\"#\"PublicApiEndpoint\": \"$PublicApiEndpoint\"#g" appsettings.json

nginx -g 'daemon off;'