const config = {
    site: 'https://localhost:44377/'
};

if (process.env.NODE_ENV === 'production'){
    config.site = 'http://api.ssl-get.site'
}

export default config;