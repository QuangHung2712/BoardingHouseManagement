<script>
export default {
    components: {},
    data(){
        return{
            titlePage: ""
        }
    },
    props: {
        title: {
            type: String,
            default: "",
        },
        pageTitle: {
            type: String,
            default: "",
        }
    },
    created(){
        this.titlePage = this.title =="" ? this.pageTitle : this.title; 
    },
    methods:{
        goBack() {
            this.$router.go(-1);  // Quay lại trang trước đó
        }
    }
};
</script>

<template>
    <v-container class="py-0 pl-8">
        <BRow class="">
            <BCol md="12">
                <ul class="breadcrumb mb-0">
                    <li class="breadcrumb-item"><h5><router-link to="/">Tìm kiếm phòng trọ</router-link></h5></li>
                    <li class="breadcrumb-item"><BLink href="javascript: void(0)" @click="goBack"></BLink></li>
                    <li class="breadcrumb-item" aria-current="page">{{ title }}</li>
                </ul>
            </BCol>
            <BCol md="12">
                <div class="page-header-title">
                    <h2 class="mb-0">{{ pageTitle }}</h2>
                </div>
            </BCol>
        </BRow>
    </v-container>
</template>