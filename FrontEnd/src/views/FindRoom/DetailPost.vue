<style scoped>
    .thumbnail-container {
        cursor: pointer;
        padding: 4px;
    }

    .thumbnail {
        border: 2px solid transparent;
        border-radius: 8px;
        transition: 0.3s;
    }

    .thumbnail.active {
        border-color: #1976d2; /* Màu xanh Vuetify */
        box-shadow: 0 0 8px rgba(25, 118, 210, 0.7);
    }

    .thumbnail {
        width: 50px;
        height: 50px; 
    }
    .item{
        background-color: #f1f4f6;
    }
    .d-grid button{
        font-size: 18px;
    }
    .customBtn{
        background-color: green;
        border-color: green;
    }
    .saved-button {
        color: red !important; /* Màu chữ trắng */
    }
</style>
<script>
    import apiClient from "@/plugins/axios";
    import { formatTablePrice } from "@/components/common/JavaScripCommon"
    import store from "../../state/store";


    export default {
        name: "LANDING",
        components: {
        },
    data() {
        return{
            length: 3,
            window: 0,
            currentSlide: 0,
            visible: false,
            index: 0,
            postData:{},
            message: '',
            snackbar: false,
            snackbarColor: '',
            customerId:0,
        }
    },
    created(){
        const postId = this.$route.params.idpost;
        this.customerId = store.getters['getCustomerId'] ?? 0;
        this.GetDetailPost(postId);
    },
    methods:{
        showImg(index) {
            this.index = index;
            this.visible = true;
        },
        handleHide() {
            this.visible = false;
        },
        GetDetailPost(postId){
            apiClient.get(`/Post/GetDetailFindPeople?postId=${postId}&customerId=${this.customerId}`)
                .then(response=>{
                    this.postData = response.data;
                    console.log(response.data);
                })
                .catch(error=>{
                    this.message = "Lấy thông tin bài đăng bị lỗi: " + error.response?.data?.message || error;
                    this.snackbar = true;
                    this.snackbarColor = 'red';
                })
        },
        FormatPrice(price){
            return formatTablePrice (price);
        },
        GoToInfoZalo(){
            const url = 'https://zalo.me/' + this.postData.sdtZalo;
            window.open(url,'_blank');
        },
        copyCurrentURL(){
            // Lấy URL hiện tại từ trình duyệt
            const currentURL = window.location.href;

            // Sử dụng Clipboard API để sao chép
            navigator.clipboard
            .writeText(currentURL)
            .then(() => {
                // Hiển thị thông báo thành công
                this.message = "Đường dẫn đã được sao chép vào clipboard!"
                this.snackbar = true;
                this.snackbarColor = 'green';
            })
            .catch((error) => {
                // Hiển thị thông báo lỗi nếu có
                this.message = `Không thể sao chép đường dẫn: ${error}`;
                this.snackbar = true;
                this.snackbarColor = 'red';
            });
        },
        Savepost(postId,status){
            
            if (status) {
                this.message = "Xoá phòng thành công";
            } else {
                this.message = "Lưu phòng thành công";
            }
            // Đặt trạng thái đang xử lý
            this.postData.isProcessing = true;
            // Gửi yêu cầu API
            apiClient
            .put(`/Post/SavePost?customerId=${this.customerId}&postId=${postId}&status=${status}`)
            .then(() => {
                // Cập nhật trạng thái `isSave` sau khi API trả về thành công
                this.postData.isSave = !this.postData.isSave;

                // Hiển thị thông báo thành công
                this.snackbar = true;
                this.snackbarColor = "green";
            })
            .catch((error) => {
                // Xử lý lỗi
                this.message =
                "Lưu phòng đã xảy ra lỗi: " +
                (error.response?.data?.message || error.message || "Không xác định");
                this.snackbar = true;
                this.snackbarColor = "red";
                this.postData.isProcessing = false;
            })
            .finally(() => {
                // Đặt lại trạng thái xử lý
                this.postData.isProcessing = false;
            });
        },
    }
}
</script>
<template>
        <v-snackbar
            v-model="snackbar"
            :timeout="10000"
            class="custom-snackbar"
            :color="snackbarColor"
        >
            <h5 class="text-center">{{ message }}</h5>
        </v-snackbar>
<v-container>
    <v-row class="m-4"> 
            <v-col cols="1" xxl="1"></v-col>
            <v-col cols="10" xxl="10">
                <BRow>
                    <BCol class="col-lg-8">
                        <BCard no-body class="mt-2"  v-if="postData.pathImgRoom != 0">
                            <v-row justify="center" class="m-0" >
                                <!-- Carousel chính -->
                                <v-carousel v-model="currentSlide" hide-delimiters show-arrows="hover" height="480" class="item" >
                                    <v-carousel-item v-for="(item, index) in postData.pathImgRoom" :key="index">
                                        <v-img :src="item" @click="showImg(index)" />
                                    </v-carousel-item>
                                </v-carousel>

                                <!-- Thumbnails -->
                                <v-row justify="center" class="m-0">
                                    <div
                                    v-for="(item, index) in postData.pathImgRoom" :key="index"
                                    @click="currentSlide = index"
                                    class="thumbnail-container"
                                    >
                                    <v-img
                                        :src="item"
                                        :alt="'Ảnh phòng ' + index "
                                        class="thumbnail"
                                        :class="{ active: currentSlide === index }"
                                    />
                                    </div>
                                </v-row>
                            </v-row>
                        </BCard>
                        <BCard>
                            <BCardHeader >
                                <h4>{{ postData.name }}</h4>
                                <p><v-icon>mdi-map-marker-radius</v-icon>{{ postData.address }}</p>
                                <h3>Giá: <span style="color: red;">{{ postData.priceRoom }}</span></h3>
                            </BCardHeader>
                            <BCardBody class="p-0 py-3">
                                <h4>Giới tính: {{ postData.gender}}</h4>
                                <h4>Mô tả</h4>
                                <p>{{ postData.moTa }}</p>
                            </BCardBody>
                        </BCard>
                    </BCol>
                    <BCol class="col-lg-4">
                        <BCard class="statistics-card-1" no-body>
                            <BCardBody class="text-center">
                                <div class="chat-avtar d-inline-flex mx-auto">
                                    <img class="rounded-circle img-fluid wid-90 img-thumbnail"
                                        :src="postData.customerAvata" alt="User image">
                                </div>
                                <h5 class="mb-0">{{ postData.customerName }}</h5>
                                <div class="d-grid gap-2 mt-3 ">
                                    <BButton class="customBtn"><v-icon class="mr-2">mdi-phone</v-icon>{{ postData.sdtCustomer }}</BButton>
                                </div>
                                <div class="d-grid gap-2 mt-3">
                                    <BButton variant="primary" @click="GoToInfoZalo"><v-icon class="mr-2">mdi-chat-processing-outline</v-icon>Nhắn tin Zalo</BButton>
                                </div>
                                <div class="d-flex justify-space-evenly mt-3">
                                    <!-- <BButton variant="white" class="p-0"><v-icon >mdi-heart-outline</v-icon>Lưu phòng</BButton> -->
                                    <BButton variant="white" class="p-0" :class="{'saved-button': postData.isSave}" @click="Savepost(postData.id,postData.isSave)">
                                        <v-icon class="mr-2" v-if="!postData.isProcessing">mdi-heart</v-icon>
                                        
                                        <v-progress-circular
                                        v-else
                                        indeterminate
                                        color="white"
                                        size="20"
                                        ></v-progress-circular>
                                        Lưu phòng
                                    </BButton>
                                    <BButton variant="white" class="p-0" @click="copyCurrentURL"><v-icon class="mr-2">mdi-share-variant</v-icon>Chia sẻ phòng</BButton>
                                </div>
                            </BCardBody>
                        </BCard>
                    </BCol>
                </BRow>
            </v-col>
            <v-col cols="1" xxl="1"></v-col>
        </v-row>
</v-container>
    <vue-easy-lightbox :visible="visible" :imgs="postData.pathImgRoom" :index="index" @hide="handleHide"></vue-easy-lightbox>
</template>