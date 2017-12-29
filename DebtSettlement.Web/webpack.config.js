/* global __dirname */

var path = require('path');

var webpack = require('webpack');
var CopyWebpackPlugin = require('copy-webpack-plugin');

const Uglify = require("uglifyjs-webpack-plugin");
var dir_js = path.resolve(__dirname, 'app/src');
var dir_html = path.resolve(__dirname, 'app/html');
var dir_build = path.resolve(__dirname, 'app/dist');
//let webApiPatch = '../../Notifications.WebAPI/ui';

module.exports = {
    entry: path.resolve(dir_js, 'index.js'),
    output: {
        path: dir_build,
        filename: 'index.js'
    },
    devServer: {
        contentBase: dir_build,
    },
    module: {
        loaders: [
            {
                loader: 'babel-loader',
                test: dir_js,
            }, {
                test: /\.html$/,
                loader: 'raw-loader!html-minifier-loader'
            }
        ]
    },
    'html-minifier-loader': {
        removeComments: true,
        collapseWhitespace: true,
        conservativeCollapse: true,
        preserveLineBreaks: false,
        minifyCSS: true
    },
    plugins: [
        new Uglify(),
        // Simply copies the files over
        new CopyWebpackPlugin([
            //{from: dir_build, to: webApiPatch }
        ]),
        // Avoid publishing files when compilation fails
        new webpack.NoErrorsPlugin()
    ],
    stats: {
        // Nice colored output
        colors: true
    },
    // Create Sourcemaps for the bundle
    devtool: 'source-map',
};
