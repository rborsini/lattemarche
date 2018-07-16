<template>
    <input class="form-control" type="time" v-model="time" @keydown="onKeyDown" />
</template>

<script>
    import Vue from 'vue';

    export default {
        props: {
            value: { type: String, default: "" },
            //max: { type: Number, default: 5 } //per impostare un valore massimo di caratteri
        },
        computed: {
            time: {
                get() { return this.value },
                set(v) { this.$emit("input", v) }
            }
        },
        methods: {
            onKeyDown(evt) {
                //if (this.value.length >= this.max) { //Abilitare se si vuole impostare un valore massimo di caratteri
                if ((evt.keyCode >= 48 && evt.keyCode <= 57) || (evt.keyCode >= 37 && evt.keyCode <= 40) || (evt.keyCode >= 96 && evt.keyCode <= 105) || evt.keyCode == 190 || evt.keyCode == 9) {
                    return
                } else {
                    evt.preventDefault()
                    return
                }
                //}
            },

        }
    }

</script>