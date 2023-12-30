# 1. For build React app
FROM node:21-alpine3.18 AS build

WORKDIR /app

COPY . /app

RUN npm run build

# 2. For Nginx setup
FROM nginx:1.25.3-alpine

COPY --from=build /app/.nginx/nginx.conf /etc/nginx/conf.d/default.conf

WORKDIR /usr/share/nginx/html

RUN rm -rf ./*

COPY --from=build /app/build .

ENTRYPOINT ["nginx", "-g", "daemon off;"]