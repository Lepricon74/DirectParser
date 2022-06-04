const path = require("path");
   
module.exports = {
    mode: "development",
    entry: "./app.jsx", // входная точка - исходный файл
    devtool: 'source-map',
    output:{
        path: path.resolve(__dirname, "./dist"),     // путь к каталогу выходных файлов - папка public
        publicPath: "/dist/",
        filename: "bundle.js"       // название создаваемого файла
    },

    devServer: {
     historyApiFallback: true,
     static: {
      directory: path.join(__dirname, "/"),
    },
     port: 8081,
     open: true,
     headers: {
        "Access-Control-Allow-Origin": "*",
        "Access-Control-Allow-Methods": "GET, POST, PUT, DELETE, PATCH, OPTIONS",
        "Access-Control-Allow-Headers": "Origin, X-Requested-With, Content-Type, Accept, Authorization"
    },
     proxy: {
         '/api': {
             target: 'http://192.168.1.243:90'
        }
    }
   },
    module:{
        rules:[   //загрузчик для jsx
            {
                test: /\.jsx?$/, // определяем тип файлов
                exclude: /(node_modules)/,  // исключаем из обработки папку node_modules
                loader: "babel-loader",   // определяем загрузчик
                options:{
                    presets:[ "@babel/preset-react"]    // используемые плагины
                }
            }
        ]
    }
}