const proxy = [
  {
    context: ['/Cep'],
    target: 'https://localhost:7102',
    secure: false,
    changeOrigin: true,
    pathRewrite: { '^/': '' }
  }
];
module.exports = proxy;
