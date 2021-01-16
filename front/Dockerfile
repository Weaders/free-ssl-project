FROM node:10 AS builder

WORKDIR /app

COPY . .

RUN npm install && npm run build

WORKDIR /var

RUN cp -r /app/build/ /var/www/

ENTRYPOINT ["tail", "-f", "/dev/null"]