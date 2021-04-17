FROM node:10 AS builder

WORKDIR /app

COPY . .

RUN npm i

CMD ["npm", "run", "build"]