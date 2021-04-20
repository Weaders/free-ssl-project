FROM node:10 AS builder

WORKDIR /app

COPY . .

RUN npm ci --silent
RUN npm install react-scripts -g --silent

CMD ["npm", "run", "build"]