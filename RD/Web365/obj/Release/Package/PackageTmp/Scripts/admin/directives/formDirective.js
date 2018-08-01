'use strict';
web365app.directive('toascii', function ($timeout) {
    return {
        restrict: 'A',
        link: function (scope, element, attrs) {
            $timeout(function () {

                element[0].addEventListener('keyup', function () {
                    $('input[name="' + attrs.field + '"]').val(Web365Utility.removeUnicode(element[0].value));
                }, false)

            }, 0);
        }
    };
});

'use strict';
web365app.directive('spinner', function ($timeout) {
    return {
        restrict: 'A',
        link: function (scope, element, attrs) {
            $timeout(function () {

                $(element).ace_spinner({ value: $(element).val() == '' ? 0 : parseInt($(element).val()), min: 0, max: 10000, step: 1, icon_up: 'icon-plus', icon_down: 'icon-minus', btn_up_class: 'btn-success', btn_down_class: 'btn-danger' }).css({ width: '70px' });

            }, 0);
        }
    };
});

'use strict';
web365app.directive('maskmoney', function ($timeout) {
    return {
        restrict: 'A',
        link: function (scope, element, attrs) {

            $timeout(function () {

                $(element).parent().prepend('<input type="hidden" name="' + attrs.dataname + '" value="' + element[0].value.replace(/\./g, '') + '" />');

                element[0].addEventListener('keyup', function () {
                    $(element).parent().find('input:hidden').val(element[0].value.replace(/\./g, ''));
                }, false)

                $(element).maskMoney({
                    thousands: '.',
                    decimal: ',',
                    precision: 0
                });

                $(element).maskMoney('mask', element[0].value);

            }, 0);

        }
    };
});

'use strict';
web365app.directive('acedatepicker', function ($timeout) {
    return {
        restrict: 'A',
        require: '^?ngModel',
        link: function (scope, element, attrs, ngModel) {

            $timeout(function () {               

                $(element).datepicker().on('changeDate', function (e) {

                    scope.$apply(function () {
                        if (typeof ngModel !== 'undefined') {
                            ngModel.$setViewValue(e.date.toDateString());
                        }
                    });                    

                }).next().on(ace.click_event, function (e) {                    
                    $(this).prev().focus();
                });

            }, 0);

        }
    };
});

'use strict';
web365app.directive('sortable', ['$timeout', function ($timeout) {
    return {
        restrict: 'A',
        scope: {
            dataid: '@',
            dataname: '@'
        },
        link: function (scope, element, attrs) {

            $timeout(function () {

                if (typeof scope.dataname !== 'undefined' && typeof scope.dataid !== 'undefined') {
                    $(element).parent().append('<input type="hidden" name="' + scope.dataname + '" value="' + scope.dataid + '" />');
                }

                $(element).sortable({
                    connectWith: '.list-sort-able',
                    items: ' .item-sort-able',
                    opacity: 0.8,
                    revert: true,
                    forceHelperSize: true,
                    forcePlaceholderSize: true,
                    tolerance: 'pointer',
                    update: function (event, ui) {

                        scope.sortData();
                    }
                });                

            }, 0);

            scope.sortData = function () {

                scope.arrId = [];

                $(element).find('.item-sort-able').each(function (i) {
                    scope.arrId.push($(this).attr('data-id'));
                });

                $(element).find('input:hidden').val(scope.arrId);

                if (typeof scope.dataname !== 'undefined' && typeof scope.dataid !== 'undefined') {
                    $(element).find('input:hidden[name="' + scope.dataname + '"]').val(scope.arrId);
                }

            };

        }
    };
}]);



'use strict';
web365app.directive('colorbox', function () {
    return {
        restrict: 'A',
        link: function (scope, element, attrs) {

            $(element).colorbox({
                reposition: true,
                scalePhotos: true,
                scrolling: false,
                previous: '<i class="icon-arrow-left"></i>',
                next: '<i class="icon-arrow-right"></i>',
                close: '&times;',
                current: '{current} of {total}',
                maxWidth: '100%',
                maxHeight: '100%',
                onOpen: function () {
                    document.body.style.overflow = 'hidden';
                },
                onClosed: function () {
                    document.body.style.overflow = 'auto';
                },
                onComplete: function () {
                    $.colorbox.resize();
                }
            });

        }
    };
});

'use strict';
web365app.directive('chosen', function ($timeout) {
    return {
        restrict: 'A',
        transclude: true,
        require: '^?ngModel',
        scope: {
            list: '='
        },
        link: function (scope, element, attrs, ngModel) {

            if (typeof attrs.dataname !== 'undefined') {

                $(element).parent().prepend('<input type="hidden" name="' + attrs.dataname + '" value="' + attrs.datavalue + '" />');

            }

            scope.listId = attrs.datavalue != '' && typeof attrs.datavalue !== 'undefined' ? attrs.datavalue.split(',') : [];

            scope.$watch('list', function () {

                if (typeof scope.list !== 'undefined') {

                    $timeout(function () {

                        angular.forEach(scope.list, function (v, k) {

                            angular.forEach(scope.listId, function (v1, k1) {

                                if (v.ID == v1) {

                                    $(element).find('option[value="' + k + '"]').attr('selected', '');

                                }

                            });

                        });

                        $(element).chosen({
                            width: '100%',
                            allow_single_deselect: true,
                            placeholder_text_single: attrs.placeholder,
                            no_results_text: '...'
                        }).change(function (evt, params) {

                            if (typeof params !== 'undefined') {

                                if (typeof params.selected !== 'undefined') {

                                    scope.changeListId(scope.list[params.selected].ID.toString());

                                }

                                if (typeof params.deselected !== 'undefined') {

                                    scope.changeListId(scope.list[params.deselected].ID.toString());

                                }

                            } else {

                                scope.changeListId();

                            }

                        });

                    }, 0);

                }

            });

            scope.changeListId = function (id) {

                if (scope.listId.indexOf(id) == -1) {

                    if (typeof attrs.multiple === 'undefined') {

                        scope.listId.length = 0;

                    }

                    scope.listId.push(id);

                } else {

                    scope.listId = $.grep(scope.listId, function (n, i) {
                        return (n !== id);
                    });

                }

                $timeout(function () {
                    scope.$apply(function () {
                        if (typeof ngModel !== 'undefined') {
                            ngModel.$setViewValue(scope.listId.length > 0 ? scope.listId[0] : null);
                        }
                    });
                }, 0);

                $('input:hidden[name="' + attrs.dataname + '"]').val(scope.listId);

            };

        }
    };
});

'use strict';
web365app.directive('uploadFile', function () {
    return {
        restrict: 'A',
        link: function (scope, element, attrs) {

            var name = attrs.dataname;

            $(element).parent().append('<input type="hidden" name="' + name + '" value="' + attrs.value + '" />');

            $(element).ace_file_input({
                style: 'well',
                btn_choose: 'Bạn có thể kéo ảnh vào đây',
                btn_change: null,
                no_icon: 'icon-cloud-upload',
                droppable: true,
                thumbnail: 'small',
                preview_error: function (filename, error_code) {

                }
            }).on('change', function () {
                for (var i = 0; i < this.files.length ; i++) {
                    var data = new FormData();
                    data.append('file', this.files[i]);
                    $.ajax({
                        type: "POST",
                        url: "/Upload/UploadFile.ashx",
                        enctype: 'multipart/form-data',
                        data: data,
                        processData: false,
                        contentType: false,
                        xhr: function () {

                            var myXhr = $.ajaxSettings.xhr();

                            if (myXhr.upload) {
                                myXhr.upload.addEventListener('progress', function (e) {
                                    var done = e.position || e.loaded, total = e.totalSize || e.total;
                                }, false);
                            }
                            return myXhr;
                        },
                        success: function (data, textStatus, jqXHR) {

                            $('input:hidden[name="' + name + '"]').val(data);

                        },
                        error: function (jqXHR, textStatus, errorThrown) {
                            console.log(jqXHR);
                        }
                    });
                }
            });
        }
    };
});

'use strict';
web365app.directive('uploadImages', function (utilityServices, pictureService, pictureTypeService) {
    return {
        restrict: 'A',
        link: function (scope, element, attrs) {

            scope.listTreeTypePicture = [];

            pictureTypeService.getListForTree().then(function (response) {

                scope.listTreeTypePicture = response.list;

            }, function (err) {
                console.log(err.error_description);
            });

            utilityServices.loadTemplate('/Angular/Views/Admin/Picture/preview-upload-picture.html').then(function (res) {

                $(element).find('.dropzone').dropzone({
                    url: "/Upload/UploadFile.ashx",
                    addRemoveLinks: true,
                    previewTemplate: res,
                    success: function (file, res, e) {

                        if (file.previewElement) {

                            file.previewElement.classList.add("dz-success");

                            $(file.previewElement).find('input:hidden[name="FileName"]').val(res);

                            $(file.previewElement).find('input:text[name="Name"]').val(res);

                            $(file.previewElement).find('input:text[name="Alt"]').val(res);

                            $(file.previewElement).find('textarea[name="Summary"]').val(res);

                        }

                    }
                });

            });

            scope.saveImages = function () {

                var total = 0;

                var typeId = $(element).find('input:hidden[name="TypeID"]').val();

                if (typeId == '' || typeId == null || typeof typeId === 'undefined ') {

                    utilityServices.notificationWarning('Thông báo', 'Bạn cần chọn nhóm ảnh');

                } else {

                    $(element).find('.upload-picture').each(function (i) {

                        $(this).prepend('<input type="hidden" name="TypeID" value="' + typeId + '" />');

                        pictureService.modified($(this).serialize()).then(function (res) {

                            total += 1;

                            if (total == $(element).find('.upload-picture').size()) {

                                scope.closeUploadImages();

                            }

                        });

                    });

                }

            };

            scope.closeUploadImages = function () {

                scope.modalUploadPicture.dismiss('cancel');

                if (typeof scope.selectPicture !== 'undefined') {

                    scope.selectPicture();

                }

                if (typeof scope.resetListPicture !== 'undefined') {

                    scope.resetListPicture();

                }

            };
        }
    };
});

'use strict';
web365app.directive('ckEditor', ['$timeout', function ($timeout) {
    return {
        require: '?ngModel',
        link: function (scope, elm, attr, ngModel) {

            var ck = CKEDITOR.replace(elm[0]);

            ck.on('pasteState', function () {
                ck.updateElement();
                scope.$apply(function () {
                    ngModel.$setViewValue(ck.getData());
                });
            });

            ngModel.$render = function (value) {

                ck.setData(ngModel.$modelValue);

                ck.updateElement();

                $timeout(function () {
                    scope.$apply(function () {
                        ngModel.$setViewValue(ck.getData());
                    });
                }, 0);
            };
        }
    };
}])

'use strict';
web365app.directive('wysiwyg', function () {
    return {
        restrict: 'A',
        link: function (scope, element, attrs) {
            $(element).ace_wysiwyg({
                toolbar:
                [
                    'font',
                    null,
                    'fontSize',
                    null,
                    { name: 'bold', className: 'btn-info' },
                    { name: 'italic', className: 'btn-info' },
                    { name: 'strikethrough', className: 'btn-info' },
                    { name: 'underline', className: 'btn-info' },
                    null,
                    { name: 'insertunorderedlist', className: 'btn-success' },
                    { name: 'insertorderedlist', className: 'btn-success' },
                    { name: 'outdent', className: 'btn-purple' },
                    { name: 'indent', className: 'btn-purple' },
                    null,
                    { name: 'justifyleft', className: 'btn-primary' },
                    { name: 'justifycenter', className: 'btn-primary' },
                    { name: 'justifyright', className: 'btn-primary' },
                    { name: 'justifyfull', className: 'btn-inverse' },
                    null,
                    { name: 'createLink', className: 'btn-pink' },
                    { name: 'unlink', className: 'btn-pink' },
                    null,
                    { name: 'insertImage', className: 'btn-success' },
                    null,
                    'foreColor',
                    null,
                    { name: 'undo', className: 'btn-grey' },
                    { name: 'redo', className: 'btn-grey' }
                ],
                'wysiwyg': {
                    fileUploadError: function () {
                        console.log('Upload file failed');
                    }
                }
            }).prev().addClass('wysiwyg-style2');

            element[0].addEventListener('DOMSubtreeModified', function () {

                $(element[0].parentElement).find('input:hidden:first-child').val(element[0].innerHTML);

            }, false);
        }
    };
});

'use strict';
web365app.directive('tree', function ($timeout, $compile) {
    return {
        restrict: 'E',
        transclude: true,
        require: '^?ngModel',
        scope: {
            list: '=',
            name: '@',
            value: '@',
            showAction: '@',
            selectOne: '@',
            multiselect: '@',
            title: '@',
            collapsed: '@',
            fieldId: '@',
            fieldName: '@',
            fieldParent: '@',
            selected: '&',
            detail: '&'
        },
        template: '<div class="widget-box {{collapsed}}">'
                    + '<div class="widget-header widget-header-small header-color-blue2">'
                        + '<h6>{{title}}</h6>'
                        + '<div class="widget-toolbar" ng-if="collapsed != null"><a href="javascript:;" data-action="collapse"><i class="icon-chevron-down"></i></a></div>'
                    + '</div>'
                    + '<div class="widget-body">'
                        + '<div class="widget-main padding-8">'
                            + '<div class="tree tree-selectable">'
                            + '</div>'
                            + '<input name="{{name}}" type="hidden" value="{{value}}" />'
                        + '</div>'
                    + '</div>'
                + '</div>',
        link: function (scope, element, attrs, ngModel) {

            scope.getFieldId = scope.fieldId == '' || typeof scope.fieldId === 'undefined' ? 'ID' : scope.fieldId;

            scope.getFieldName = scope.fieldName == '' || typeof scope.fieldName === 'undefined' ? 'Name' : scope.fieldName;

            scope.getFieldParent = scope.fieldParent == '' || typeof scope.fieldParent === 'undefined' ? 'Parent' : scope.fieldParent;

            scope.building = false;

            scope.content = '';

            scope.listTree = [];

            scope.$watch('list', function (n, o) {

                $timeout(function () {

                    if (!scope.building || (o != null && o.length == 0) || typeof n !== 'undefined') {
                        scope.renderTree();
                    }

                    scope.building = true;

                }, 0);

            });

            scope.buildTree = function (listTree) {

                var strTree = '';

                angular.forEach(listTree, function (value, key) {                    

                    if (value.listChild.length > 0) {
                        strTree += '<div class="tree-folder">'
                        + '<div class="tree-folder-header" data-id="' + value[scope.getFieldId] + '" ng-click="headerClick($event)">'
                            + '<i class="icon-plus"></i>'
                            + (scope.selectOne == 'true' || scope.multiselect == 'true' ? '<i class="' + (scope.value != null && scope.value.split(',').indexOf(value[scope.getFieldId].toString()) > -1 ? "icon-ok select-folder selected-folder" : "icon-remove select-folder") + '" ng-click="folderSelectClick($event)"></i>' : '')
                            + '<div class="tree-folder-name">' + value[scope.getFieldName] + '</div>'
                            + (scope.showAction == 'true' ? '<i class="icon-zoom-in blue detail-item-tree" ng-click="detailItemTreeClick($event)"></i>' : '')
                        + '</div>'
                        + '<div class="tree-folder-content tree-hidden">' + scope.buildTree(value.listChild) + '</div>'
                        + '</div>';
                    }
                    else {
                        strTree += '<div class="tree-item' + (scope.value != null && scope.value.split(',').indexOf(value[scope.getFieldId].toString()) > -1 ? " tree-selected" : "") + '" data-id="' + value[scope.getFieldId] + '" ng-click="treeItemClick($event)">'
                            + (scope.selectOne == 'true' || scope.multiselect == 'true' ? '<i class="' + (scope.value != null && scope.value.split(',').indexOf(value[scope.getFieldId].toString()) > -1 ? "icon-ok" : "icon-remove") + '"></i>' : '')
                            + '<div class="tree-item-name">' + value[scope.getFieldName] + '</div>'
                            + (scope.showAction == 'true' ? '<i class="icon-zoom-in blue detail-item-tree" ng-click="detailItemTreeClick($event)"></i>' : '')
                        + '</div>';
                    }

                });

                return strTree;
            };

            scope.renderTree = function () {

                angular.forEach(scope.list, function (value, key) {

                    if (value[scope.getFieldParent] == null) {

                        scope.listTree.push(value);
                    }

                });

                scope.buildListTree(scope.listTree);

                if (scope.value != null) {
                    scope.getNameSelected(scope.value);
                }

                scope.content = scope.buildTree(scope.listTree);

                $(element[0]).find('.tree').html(scope.content);

                $compile(element.contents())(scope);

            };

            scope.buildListTree = function (listParent) {

                angular.forEach(listParent, function (v1, k1) {

                    v1.listChild = [];

                    angular.forEach(scope.list, function (v2, k2) {
                        if (v2[scope.getFieldParent] == v1[scope.getFieldId]) {
                            v1.listChild.push(v2);
                        }
                    });

                    scope.buildListTree(v1.listChild);

                });
            };

            scope.getNameSelected = function (listId) {

                var str = '';

                angular.forEach(scope.list, function (v, k) {

                    if (listId.toString().split(',').indexOf(v[scope.getFieldId].toString()) > -1) {
                        str += str == '' ? v[scope.getFieldName] : ', ' + v[scope.getFieldName];
                    }

                });

                scope.title = str != '' ? str : scope.title;
            };

            scope.headerClick = function ($event) {

                $($event.target).closest('.tree-folder').find('.tree-folder-content:first').toggleClass('tree-hidden');
                $($event.target).closest('.tree-folder').find('i:first').toggleClass('icon-plus').toggleClass('icon-minus');

            };

            scope.treeItemClick = function ($event) {

                var _self = $($event.target).hasClass('tree-item') ? $($event.target) : $($event.target).closest('.tree-item');

                if (_self.hasClass('tree-selected') == false) {
                    scope.cleanSeleted();
                }

                if (scope.selectOne == 'true' || scope.multiselect == 'true') {
                    _self.toggleClass('tree-selected');
                    _self.find('i').toggleClass('icon-remove').toggleClass('icon-ok');

                    if (_self.find('i').hasClass('icon-ok') && scope.multiselect == 'true') {
                        _self.parents('.tree-folder').each(function (i) {

                            $(this).find('.select-folder:first').removeClass('icon-remove').addClass('icon-ok').addClass('selected-folder');

                        });
                    }

                    scope.getSelected();
                }

                scope.selected();

            };

            scope.detailItemTreeClick = function ($event) {

                $event.stopPropagation();

                var id = $($event.target).parent().attr('data-id');

                scope.detail({ id: id });

            };

            scope.folderSelectClick = function ($event) {

                $event.stopPropagation();

                if ($($event.target).hasClass('icon-ok') == false) {

                    $($event.target).parents('.tree-folder:not(:first)').each(function (i) {

                        $(this).find('.select-folder:first').removeClass('icon-remove').addClass('icon-ok').addClass('selected-folder');

                    });

                    scope.cleanSeleted();
                }

                if ($($event.target).hasClass('icon-ok')) {
                    $($event.target).closest('.tree-folder').find('.selected-folder:not(:first)').removeClass('icon-ok').removeClass('selected-folder').addClass('icon-remove');
                    $($event.target).closest('.tree-folder').find('.tree-item').removeClass('tree-selected');
                    $($event.target).closest('.tree-folder').find('.tree-item .icon-ok').removeClass('icon-ok').addClass('icon-remove');
                }

                $($event.target).toggleClass('icon-remove').toggleClass('icon-ok').toggleClass('selected-folder');

                scope.getSelected();

                scope.selected();

            };

            scope.cleanSeleted = function () {

                if (scope.selectOne == 'true') {
                    $(element[0]).find('.select-folder').removeClass('icon-ok selected-folder').addClass('icon-remove');
                    $(element[0]).find('.tree-item').removeClass('tree-selected');
                    $(element[0]).find('.tree-item').find('i').removeClass('icon-ok').addClass('icon-remove');
                }

            };

            scope.getSelected = function () {

                var listId = [];

                $(element[0]).find('.icon-ok').each(function (i) {
                    listId.push(parseInt($(this).parent().attr('data-id')));
                });

                $(element[0]).find('input[name="' + scope.name + '"]').val(listId);

                $timeout(function () {
                    scope.$apply(function () {
                        if (typeof ngModel !== 'undefined') {
                            ngModel.$setViewValue(listId.length > 0 ? listId[0] : null);
                        }
                    });
                }, 0);

                scope.getNameSelected(listId);

            };

        }
    };
});

'use strict';
web365app.directive('boxPicture', function ($modal, $compile, pictureTypeService, pictureService, utilityServices) {
    return {
        restrict: 'E',
        transclude: true,
        scope: {
            arrid: '=',
            only: '@',
            multi: '@',
            property: '@'
        },
        templateUrl: 'Angular/Views/Admin/Picture/block-picture.html',
        link: function (scope, element, attrs) {

            scope.arrid = scope.arrid != null ? scope.arrid : '';

            $(element).append('<input type="hidden" name="' + scope.property + '" value="' + scope.arrid + '" />');

            var hiddenValue = $('input:hidden[name="' + scope.property + '"]');

            scope.listId = hiddenValue.val() != '' ? hiddenValue.val().split(',') : [];

            scope.listPicture = [];

            scope.listSelected = [];

            scope.totalPicture = 0;

            scope.pictureFilter = {
                name: '',
                fileName: '',
                typePictureId: null,
                currentRecord: 0,
                numberRecord: 20,
                currentPage: 1,
                propertyNameSort: 'ID',
                descending: true,
                isShow: true
            };

            if (scope.arrid != null) {
                pictureService.getListByArrayID(scope.arrid).then(function (res) {
                    scope.listSelected = res.list;
                });
            }

            scope.loadPicture = function () {

                var typePictureId = $('input:hidden[name="typePictureId"]').val();

                scope.pictureFilter.typeId = typePictureId != '' && typePictureId != null ? typePictureId : null;

                scope.pictureFilter.currentRecord = (scope.pictureFilter.currentPage - 1) * scope.pictureFilter.numberRecord;

                pictureService.getList(scope.pictureFilter).then(function (response) {

                    if (scope.listPicture.length == 0) {

                        scope.totalPicture = response.total;

                        scope.listPicture = response.list;

                    } else {

                        scope.listPicture = scope.listPicture.concat(response.list);

                    }

                    scope.pictureFilter.currentPage += 1;

                }, function (err) {

                    console.log(err.error_description);

                });

            };

            scope.searchPicture = function () {

                scope.pictureFilter.currentPage = 1;

                scope.listPicture = [];

                scope.loadPicture();

            };

            scope.treeSelected = function () {

                scope.pictureFilter.currentPage = 1;

                scope.listPicture = [];

                scope.loadPicture();

            };

            scope.loadMorePicture = function () {

                scope.loadPicture();

            };

            scope.selectPicture = function () {

                pictureTypeService.getListForTree().then(function (response) {

                    scope.treeTypePicture = response.list;

                    scope.modalSelectPicture = $modal.open({
                        templateUrl: 'Angular/Views/Admin/Picture/select-picture.html',
                        scope: scope
                    });

                    scope.loadPicture();

                }, function (err) {
                    console.log(err.error_description);
                });

            };

            scope.choicePicture = function ($event, item) {

                var newItem = $compile('<li class="item-sort-able" data-id="' + item.ID + '">' +
                                '<a href="/UploadFile/ImageThumbs/' + item.FileName + '" data-rel="colorbox">' +
                                    '<img alt="' + item.Alt + '" src="/UploadFile/ImageThumbs/' + item.FileName + '" />' +
                                '</a>' +
                                '<div class="tools tools-bottom">' +
                                    '<a href="javascript:;" ng-click="removePicture($event, ' + item.ID + ')">' +
                                        '<i class="icon-remove red"></i>' +
                                    '</a>' +
                                '</div>' +
                            '</li>')(scope);

                if (scope.only == 'true') {

                    hiddenValue.val(item.ID);

                    $(element).find('.item-sort-able').remove();

                    $(element).find('.list-sort-able').prepend(newItem);

                    scope.closeSelectPictureModal();

                }

                if (scope.multi == 'true') {                    

                    if (scope.listId.indexOf(item.ID.toString()) == -1) {

                        scope.listId.push(item.ID.toString());

                        $(element).find('.list-sort-able').prepend(newItem);

                        utilityServices.notificationSuccess('Thông báo', 'Lựa chọn thành công');

                    } else {

                        scope.listId = $.grep(scope.listId, function (n, i) {
                            return (n !== item.ID.toString());
                        });

                        $(element).find('.list-sort-able li[data-id="' + item.ID + '"]').remove();

                        utilityServices.notificationSuccess('Thông báo', 'Bỏ chọn thành công');

                    }

                    hiddenValue.val(scope.listId);

                    var _self = $event.target.localName == 'i' ? $($event.target) : $($event.target).find('i');

                    _self.toggleClass('icon-check-empty').toggleClass('icon-check');

                }
            };

            scope.removePicture = function ($event, id) {

                scope.listId = $.grep(scope.listId, function (n, i) {
                    return (n !== id.toString());
                });

                hiddenValue.val(scope.listId);

                $($event.target).closest('li').remove();

            };

            scope.addNewPicture = function () {

                scope.closeSelectPictureModal();

                scope.modalUploadPicture = $modal.open({
                    templateUrl: 'Angular/Views/Admin/Picture/upload-images.html',
                    scope: scope
                });

            };

            scope.closeSelectPictureModal = function () {

                scope.pictureFilter.currentRecord = 0;

                scope.pictureFilter.currentPage = 1;

                scope.listPicture = [];

                scope.modalSelectPicture.dismiss('cancel');

            };
        }
    };
});