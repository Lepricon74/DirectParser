const path = require("path");
   
module.exports = {
    mode: "development",
    entry: "./app.jsx",
    devtool: 'source-map',
    output:{
        path: path.resolve(__dirname, "./dist"),
        publicPath: "/dist/",
        filename: "bundle.js"
    },

    devServer: {
     historyApiFallback: true,
     static: {
      directory: path.join(__dirname, "/"),
    },
     port: 8082,
     open: true,
     proxy: {
         '/api': {
             target: 'http://89.108.88.254:84',
             //target: 'http://localhost:5010',
             secure: false
        }
    }
   },
    module:{
        rules:[
            {
                test: /\.jsx?$/,
                exclude: /(node_modules)/,
                loader: "babel-loader",
                options:{
                    presets:[ "@babel/preset-react"]
                }
            }
        ]
    }
}