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
    <div class="page-header">
        <div class="page-block">
            <BRow class="align-items-center">
                <BCol md="12">
                    <ul class="breadcrumb">
                        <li class="breadcrumb-item"><router-link to="/tower">Toà nhà</router-link></li>
                        <li class="breadcrumb-item"><BLink href="javascript: void(0)" @click="goBack">{{ pageTitle }}</BLink></li>
                        <li class="breadcrumb-item" aria-current="page">{{ title }}</li>
                    </ul>
                </BCol>
                <BCol md="12">
                    <div class="page-header-title">
                        <h2 class="mb-0">{{ titlePage }}</h2>
                    </div>
                </BCol>
            </BRow>
        </div>
    </div>
</template>