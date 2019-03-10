// Karma configuration
// Generated on Fri Feb 19 2016 22:16:19 GMT+0300 (RTZ 2 (зима))

module.exports = function(config) {
  config.set({

    // base path that will be used to resolve all patterns (eg. files, exclude)
    basePath: '',


    // frameworks to use
    // available frameworks: https://npmjs.org/browse/keyword/karma-adapter
    frameworks: ['jasmine'],


    // list of files / patterns to load in the browser
    files: [
        { pattern: 'vendors/jquery/jquery-2.1.4.js', watched: false },
        { pattern: 'vendors/angular/angular.js', watched: false },
        { pattern: 'vendors/angular/angular-mocks.js', watched: false },
        { pattern: 'vendors/angular/*.js', watched: false },
        { pattern: 'vendors/moment/moment.js', watched: false },
        'vendors/!(ckeditor)/*.js',
        'vendors/!(ckeditor)/**/*.js',
        'scripts/_common/*/*.js',
        'scripts/_partials/*/*.js',
        'scripts/_mobile/*/*.js',
        'scripts/_common/**/*.js',
        'scripts/_partials/**/*.js',
        'scripts/_mobile/**/*.js',
        'scripts/*/!(app|dependency)*.js',
        'scripts/**/!(app|dependency)*.js',
        'scripts/dependency.js',
        'scripts/app.js',
    ],

    // list of files to exclude
    exclude: [
    ],


    // preprocess matching files before serving them to the browser
    // available preprocessors: https://npmjs.org/browse/keyword/karma-preprocessor
    preprocessors: {
    },


    // test results reporter to use
    // possible values: 'dots', 'progress'
    // available reporters: https://npmjs.org/browse/keyword/karma-reporter
    reporters: ['progress'],


    // web server port
    port: 9876,


    // enable / disable colors in the output (reporters and logs)
    colors: true,


    // level of logging
    // possible values: config.LOG_DISABLE || config.LOG_ERROR || config.LOG_WARN || config.LOG_INFO || config.LOG_DEBUG
    logLevel: config.LOG_INFO,


    // enable / disable watching file and executing tests whenever any file changes
    autoWatch: true,


    // start these browsers
    // available browser launchers: https://npmjs.org/browse/keyword/karma-launcher
    browsers: ['Chrome'],


    // Continuous Integration mode
    // if true, Karma captures browsers, runs the tests and exits
    singleRun: false,

    // Concurrency level
    // how many browser should be started simultaneous
    concurrency: Infinity
  })
}
