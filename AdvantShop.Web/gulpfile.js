/// <binding ProjectOpened='styles:watch' />
/// <vs />
require('es6-promise').polyfill();

var PATH = {
    scss: ['admin/**/*.scss',
          'areas/**/*.scss',
          'design/**/*.scss',
          'styles/**/*.scss',
          'scripts/**/*.scss',
          //'templates/**/*.scss',
          'modules/**/*.scss',
          '../modules/**/*.scss'],
    tenplatesscss: ['templates/**/*.scss'],
    images: [
        './pictures/**/**/*',
        './pictures/*',
        './images/**/**/*',
        './design/themes/**/**/*',
        './userfiles/**/**/*'],
    inplace: {
        min: [
                './scripts/_partials/inplace/inplace.js',
                './scripts/_partials/inplace/controllers/inplaceProgressController.js',
                './scripts/_partials/inplace/controllers/inplaceSwitchController.js',
                './scripts/_partials/inplace/services/inplaceService.js',
                './scripts/_partials/inplace/directives/inplaceDirectivesMinimum.js'
        ],
        max: [
        //'./scripts/_partials/inplace/**/*.js',
        //'./scripts/_partials/inplace/*.js'
         'scripts/_partials/inplace/inplace.js',
         'scripts/_partials/inplace/directives/inplaceDirectivesMinimum.js',
         'scripts/_partials/inplace/directives/inplaceDirectives.js',
         'scripts/_partials/inplace/controllers/inplaceSwitchController.js',
         'scripts/_partials/inplace/controllers/inplaceRichController.js',
         'scripts/_partials/inplace/controllers/inplaceRichButtonsController.js',
         'scripts/_partials/inplace/controllers/inplaceModalController.js',
         'scripts/_partials/inplace/controllers/inplaceAutocompleteController.js',
         'scripts/_partials/inplace/controllers/inplaceAutocompleteButtonsController.js',
         'scripts/_partials/inplace/controllers/inplacePropertiesNewController.js',
         'scripts/_partials/inplace/controllers/inplaceImageController.js',
         'scripts/_partials/inplace/controllers/inplaceImageButtonsController.js',
         'scripts/_partials/inplace/controllers/inplacePriceController.js',
         'scripts/_partials/inplace/controllers/inplacePricePanelController.js',
         'scripts/_partials/inplace/controllers/inplacePriceButtonsController.js',
         'scripts/_partials/inplace/controllers/inplaceProgressController.js',
         'scripts/_partials/inplace/services/inplaceService.js'
        ],
        build: './scripts/_partials/inplace/build/'
    },
    logoGenerator: {
        js: [
            'vendors/tinycolor/tinycolor.js',
            'vendors/es6-promise/es6-promise.auto.min.js',
            'vendors/html2canvas/html2canvas.min.js',
            'vendors/angular-color-picker/angularjs-color-picker.js',

            'scripts/_partials/logo-generator/logoGenerator.js',
            'scripts/_partials/logo-generator/controllers/logoGeneratorController.js',
            'scripts/_partials/logo-generator/controllers/logoGeneratorFontsController.js',
            'scripts/_partials/logo-generator/controllers/logoGeneratorPreviewController.js',
            'scripts/_partials/logo-generator/controllers/logoGeneratorTriggerController.js',
            'scripts/_partials/logo-generator/controllers/logoGeneratorModalController.js',
            'scripts/_partials/logo-generator/services/logoGeneratorService.js',
            'scripts/_partials/logo-generator/components/logoGeneratorComponent.js',
            'scripts/_partials/logo-generator/filters/logoGeneratorFilter.js',
        ],
        build: './scripts/_partials/logo-generator/build/'
    }
};

var gulp = require('gulp');
var templateCache = require('gulp-angular-templatecache');
var imagemin = require('gulp-imagemin');
var pngquant = require('imagemin-pngquant');
var autoprefixer = require('gulp-autoprefixer');
var juice = require('gulp-juice');
var sass = require('gulp-sass');

//for inplace min
var concat = require('gulp-concat');
var uglify = require('gulp-uglify');
var del = require('del');
var runSequence = require('run-sequence');
var cache = require('gulp-cached');
var watch = require('gulp-watch');
var batch = require('gulp-batch');

gulp.task('default', function () {
    gulp.src(['./scripts/**/**/**/templates/*.html'])
        .pipe(templateCache('templatesCache.js', { module: 'templatesCache', standalone: true, root: '/' + './scripts/' }))
        .pipe(gulp.dest('./scripts/templatesCache/'));

    gulp.src(['./Areas/Mobile/scripts/**/**/**/templates/*.html'])
        .pipe(templateCache('templatesCache.js', { module: 'templatesCache', standalone: true, root: '/' + './Areas/Mobile/scripts/' }))
        .pipe(gulp.dest('./Areas/Mobile/scripts/templatesCache/'));

    gulp.src(['./Areas/AdminMobile/scripts/**/**/**/templates/*.html'])
        .pipe(templateCache('templatesCache.js', { module: 'templatesCache', standalone: true, root: '/' + './Areas/AdminMobile/scripts/' }))
        .pipe(gulp.dest('./Areas/AdminMobile/scripts/templatesCache/'));
});

gulp.task('adminTemplates', function () {
    return gulp.src(['./areas/admin/content/**/*.html'])
        .pipe(templateCache('templatesCache.js', { module: 'adminTemplatesCache', standalone: true, root: '../areas/admin/content/' }))
        .pipe(gulp.dest('./areas/admin/content/'));
})

gulp.task('styles', function () {
    return gulp.src(PATH.scss)
          //.pipe(watch(PATH.scss))
          .pipe(sass().on('error', sass.logError))
          .pipe(autoprefixer({
              browsers: ['last 2 versions', 'safari > 6', 'iOS > 6'],
              cascade: false
          }))
          .pipe(gulp.dest(function (file) {
              return file.base;
          }));
});
gulp.task('themplates-styles', function () {
    return gulp.src(PATH.tenplatesscss)
          .pipe(sass().on('error', sass.logError))
          .pipe(autoprefixer({
              browsers: ['last 2 versions', 'safari > 6', 'iOS > 6'],
              cascade: false
          }))
          .pipe(gulp.dest(function (file) {
              return file.base;
          }));
});
gulp.task('styles:watch', function () {
    watch(PATH.scss, batch(function (events, done) {
        gulp.start('styles', done);
    }));
});

gulp.task('optimizeImages', function () {
    return gulp.src(PATH.images)
                 .pipe(imagemin({
                     progressive: true,
                     svgoPlugins: [{ removeViewBox: false }],
                     use: [pngquant()]
                 }))
                .pipe(gulp.dest(function (file) {
                    return file.base;
                }));
});

gulp.task('html-css-inliner', function () {
    gulp.src('./htmlInliner/styled/*.html')
      .pipe(juice({}))
      .pipe(gulp.dest('./htmlInliner/inlined/'));
});


gulp.task('inplace', function (callback) {

    del(PATH.inplace.build)
        .then(function (deletedFiles) {
            console.log('Files deleted: \n', deletedFiles.join('\n'));

            gulp.src(PATH.inplace.min)
                  .pipe(concat('inplaceMinimum.js'))
                  .pipe(uglify())
                  .pipe(gulp.dest(PATH.inplace.build));


            gulp.src(PATH.inplace.max)
                    .pipe(concat('inplaceMaximum.js'))
                    .pipe(uglify())
                    .pipe(gulp.dest(PATH.inplace.build));
        })
});


gulp.task('logoGenerator', function () {
    del(PATH.logoGenerator.build)
        .then(function (paths) {
            console.log('Files deleted: \n', paths.join('\n'));

            gulp.src(PATH.logoGenerator.js)
                    .pipe(concat('logogenerator.bundle.js'))
                    .pipe(uglify())
                    .pipe(gulp.dest(PATH.logoGenerator.build));
        });
});
