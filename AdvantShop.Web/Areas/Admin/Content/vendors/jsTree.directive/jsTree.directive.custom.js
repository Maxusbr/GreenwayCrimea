/*
 * jstree.directive [http://www.jstree.com]
 * http://arvindr21.github.io/jsTree-Angular-Directive
 *
 * Copyright (c) 2014 Arvind Ravulavaru
 * Licensed under the MIT license.
 */

var ngJSTree = angular.module('jsTree.directive', []);

ngJSTree.constant('jsTree.config', {
    core: {
        themes: {
            //name: 'advantshop',
            //dots: false,
            //icons: false,
            //url: false

            name: 'advantshop',
            dots: false,
            icons: true,
            url: false
        },
        data: {
            data: function (node) {
                var self = this,
                    params = {
                        id: node.id
                    };

                if (self.element.attr('data-show-root') != null) {
                    params.showRoot = true;
                    self.element.removeAttr('data-show-root');
                }

                return params;
            }
        }
    }
});

ngJSTree.directive('jsTree', ['$compile', '$http', '$parse', 'jsTree.config', function ($compile, $http, $parse, jsTreeConfig) {

    var treeDir = {
        restrict: 'EA',
        fetchResource: function (url, cb) {
            return $http.get(url).then(function (data) {
                if (cb) cb(data.data);
            });
        },

        managePlugins: function (s, e, a, config) {
            if (a.treePlugins) {
                config.plugins = a.treePlugins.split(',');
                config.core = config.core || {};
                config.core.check_callback = config.core.check_callback || true;

                if (config.plugins.indexOf('state') >= 0) {
                    config.state = config.state || {};
                    config.state.key = a.treeStateKey;
                }

                if (config.plugins.indexOf('search') >= 0) {
                    var to = false;
                    if (e.next().attr('class') !== 'ng-tree-search') {
                        e.after('<input type="text" placeholder="Search Tree" class="ng-tree-search"/>')
                          .next()
                          .on('keyup', function (ev) {
                              if (to) {
                                  clearTimeout(to);
                              }
                              to = setTimeout(function () {
                                  treeDir.tree.jstree(true).search(ev.target.value);
                              }, 250);
                          });
                    }
                }

                if (config.plugins.indexOf('checkbox') >= 0) {
                    config.checkbox = angular.extend({}, config.checkbox, $parse(a.treeCheckbox)(s));
                    config.checkbox.keep_selected_style = false;
                }

                if (config.plugins.indexOf('contextmenu') >= 0) {
                    if (a.treeContextmenu) {
                        config.contextmenu = $parse(a.treeContextmenu)(s);
                    }
                }

                if (config.plugins.indexOf('types') >= 0) {
                    if (a.treeTypes) {
                        config.types = $parse(a.treeTypes)(s);
                    }
                }

                if (config.plugins.indexOf('dnd') >= 0) {
                    if (a.treeDnd) {
                        config.dnd = $parse(a.treeDnd)(s);
                    }
                }
            }
            return config;
        },
        manageEvents: function (s, e, a) {
            if (a.treeEvents) {
                var evMap = a.treeEvents.split(';');
                for (var i = 0; i < evMap.length; i++) {
                    if (evMap[i].length > 0) {
                        // plugins could have events with suffixes other than '.jstree'
                        var evt = evMap[i].split(':')[0];
                        if (evt.indexOf('.') < 0) {
                            evt = evt + '.jstree';
                        }

                        var cb = evMap[i].split(':')[1],
                            cbParsed = $parse(cb)(s);

                        bind(evt, cbParsed);
                    }
                }
            }

            function bind(evt, fn) {
                treeDir.tree.on(evt, function (event, data) {
                    fn(event, data);
                    s.$apply();
                });
            }
        },
        link: function (s, e, a) { // scope, element, attribute \O/
            $(function () {
                var config = {};

                // users can define 'core'
                config.core = jsTreeConfig.core;

                if (a.treeCore) {
                    config.core = $.extend(config.core, $parse(a.treeCore)(s));
                }

                // clean Case
                a.treeData = a.treeData ? a.treeData.toLowerCase() : '';
                a.treeSrc = a.treeSrc ? a.treeSrc.toLowerCase() : '';

                if (a.treeData == 'html') {
                    treeDir.fetchResource(a.treeSrc, function (data) {
                        e.html(data);
                        treeDir.init(s, e, a, config);
                    });
                } else if (a.treeData == 'json') {
                    treeDir.fetchResource(a.treeSrc, function (data) {
                        config.core.data = data;
                        treeDir.init(s, e, a, config);
                    });
                } else if (a.treeData == 'scope') {
                    s.$watch(a.treeModel, function (n, o) {
                        if (n) {
                            config.core.data = s[a.treeModel];
                            $(e).jstree('destroy');
                            treeDir.init(s, e, a, config);
                        }
                    }, true);
                    // Trigger it initally
                    // Fix issue #13
                    config.core.data = s[a.treeModel];
                    treeDir.init(s, e, a, config);
                } else if (a.treeAjax) {
                    config.core.data = config.core.data || {};
                    config.core.data.url = a.treeAjax;
                    treeDir.init(s, e, a, config);
                }
            });

        },
        init: function (s, e, a, config) {
            treeDir.managePlugins(s, e, a, config);

            var treeElement = this.tree = $(e).jstree(config);

            treeElement.on('select_node.jstree', function (evt, data) {
                if (data.event != null) {
                    $(data.event.target).closest('.jstree-node').addClass('jstree-node-selected');
                }
            });

            treeElement.on('deselect_node.jstree', function (evt, data) {
                if (data.event != null) {
                    $(data.event.target).closest('.jstree-node').removeClass('jstree-node-selected');
                }
            });

            //treeElement.on('loaded.jstree', function (evt, data) {
            //    $compile(treeElement.contents())(s);
            //});

            treeElement.on('redraw.jstree', function (evt, data) {
                if (data.nodes.length > 1) {
                    $compile(treeElement.contents())(s);
                } else {
                    $compile(treeElement.jstree().get_node(data.nodes[0].id || data.nodes[0], true))(s);
                }
            });

            treeElement.on('open_node.jstree', function (evt, data) {
                $compile(treeElement.jstree().get_node(data.node.id, true))(s);
            });

            treeDir.manageEvents(s, e, a);

            if (a.treeOnInit != null) {
                $parse(a.treeOnInit)(s, { jstree: treeElement.jstree() });
            }
        }
    };

    return treeDir;

}]);
