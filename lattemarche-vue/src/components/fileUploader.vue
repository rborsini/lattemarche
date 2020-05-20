<template>
    <input type="file" class="btn-primary fileuploader text-color" :title="title" accept="application/pdf" />
</template>

<script>

    export default {
        props: ['url', 'title'],

        // init
        mounted: function () {
            this.init(this.url);
        },

        watch: {
            url: function (value) {

                var id = "";

                if ($(this.$el).attr('id'))
                    id = '#' + $(this.$el).attr('id');
                else
                    id = this.$el;

                $(id).fileupload('option', {
                    url: value
                });


            }
        },
        methods: {
            init: function (url) {

                var id = "";

                if ($(this.$el).attr('id'))
                    id = '#' + $(this.$el).attr('id');
                else
                    id = this.$el;


                $(id).bootstrapFileInput();
                var vm = this;

                this.$upload = $(id).fileupload({

                    url: url,
                    type: 'POST',
                    start: function (e, data) {
                        $('#progress-bar').modal('show');
                    },
                    done: function (e, data) {
                        $('#progress-bar').modal('hide');

                        var obj = {
                            type: "uploaded"
                        };

                        if (data.result.MimeType) {
                            obj.Id = data.result.Id;
                            obj.OriginalFileName = data.result.OriginalFileName;
                            obj.RepositoryPath = data.result.RepositoryPath;
                            obj.RepositoryFileName = data.result.RepositoryFileName;
                            obj.Size = data.result.Size;
                            obj.MimeType = data.result.MimeType;
                            obj.ReferenceId = data.result.ReferenceId;
                            obj.ReferenceType = data.result.ReferenceType;
                            obj.Description = data.result.Description;
                            obj.Category = data.result.Category;
                        }

                        vm.$emit('uploaded', obj);

                    },
                    error: function (e) {
                        $('#progress-bar').modal('hide');
                        showError('Formato file non valido.');

                    },
                    progressall: function (e, data) {
                        var progress = parseInt(data.loaded / data.total * 100, 10);

                        $('#progress-bar .progress-bar').width(progress + '%');
                        $('#progress-bar .progress-bar span').text(progress + '%');
                    }

                });

            }
        }
    }


</script>
<style>
    .text-color{
        color:white !important
    }
</style>