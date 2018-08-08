var path = require('path')
var webpack = require('webpack')

module.exports = {
    entry: {
        // percorsi uri del progetto
        acquirenti_index_page: './src/pages/acquirenti/index.page.js',
        allevamenti_index_page: './src/pages/allevamenti/index.page.js',
        allevatori_index_page: './src/pages/allevatori/index.page.js',
        autocisterne_index_page: './src/pages/autocisterne/index.page.js',
        destinatari_index_page: './src/pages/destinatari/index.page.js',
        utenti_index_page: './src/pages/utenti/index.page.js',
        tipi_latte_index_page: './src/pages/tipi-latte/index.page.js',
        trasportatori_edit_page: './src/pages/trasportatori/edit.page.js',
        ruoli_edit_page: './src/pages/ruoli/edit.page.js',
        ruoli_new_page: './src/pages/ruoli/new.page.js',
        prelievi_latte_index_page: './src/pages/prelievi-latte/index.page.js'
    },
    output: {
        path: path.resolve(__dirname, './build'),
        filename: '[name].js'
    },
    module: {
        rules: [
            {
                test: /\.vue$/,
                loader: 'vue-loader'
            },
            {
                test: /\.js$/,
                exclude: /(node_modules|bower_components)/,
                loader: 'babel-loader',
                query: {
                    presets: ['es2015'] // Transpile the ES6 to es2015 standard
                }
            },
            {
                test: /\.tsx?$/,
                loader: 'ts-loader',
                exclude: /node_modules/,
                options: {
                    appendTsSuffixTo: [/\.vue$/],
                }
            },
            {
                test: /\.(png|jpg|gif|svg)$/,
                loader: 'file-loader',
                options: {
                    name: '[name].[ext]?[hash]'
                }
            }
        ]
    },
    resolve: {
        extensions: ['.ts', '.js', '.vue', '.json'],
        alias: {
            'vue$': 'vue/dist/vue.esm.js'
        }
    },
    devServer: {
        historyApiFallback: true,
        noInfo: true
    },
    performance: {
        hints: false
    },
    devtool: '#eval-source-map'
}

if (process.env.NODE_ENV === 'production') {
    module.exports.devtool = '#source-map'
    // http://vue-loader.vuejs.org/en/workflow/production.html
    module.exports.plugins = (module.exports.plugins || []).concat([
        new webpack.DefinePlugin({
            'process.env': {
                NODE_ENV: '"production"'
            }
        }),
        new webpack.optimize.UglifyJsPlugin({
            sourceMap: true,
            compress: {
                warnings: false
            }
        }),
        new webpack.LoaderOptionsPlugin({
            minimize: true
        })
    ])
}