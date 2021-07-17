const config = {
    site: 'http://localhost:5000/v1/'
};

if (process.env.NODE_ENV === 'production') {
    config.site = 'http://api.ssl-get.site/v1/'
}

export default config;