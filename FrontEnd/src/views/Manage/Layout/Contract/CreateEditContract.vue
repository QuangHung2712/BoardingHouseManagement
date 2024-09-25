<template>
    <V-row>
        <v-col cols="4">
            <h5>{{ title }}</h5>
        </v-col>
        <v-col offset="4" col="4">
            <v-btn>Quay về</v-btn>
            <v-btn>Lưu</v-btn>
        </v-col>
    </V-row>
</template>
<script>
    export default{
        data(){
            return{
                title: 'Thêm hợp đồng',
            }
        }
    }
</script>
