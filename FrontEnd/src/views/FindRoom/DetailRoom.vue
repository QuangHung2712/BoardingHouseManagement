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
</style>
<script>
    import pageheader from "@/components/PageHeader.vue";
    import apiClient from "@/plugins/axios";
    import VueEasyLightbox from "vue-easy-lightbox";
    import { formatTablePrice } from "@/components/common/JavaScripCommon"


    export default {
        name: "LANDING",
        components: {
            pageheader,VueEasyLightbox
        },
    data() {
        return{
            length: 3,
            window: 0,
            currentSlide: 0,
            visible: false,
            index: 0,
            roomData:{},
            message: '',
            snackbar: false,
            snackbarColor: '',
        }
    },
    created(){
        const roomId = this.$route.params.idroom;
        this.GetDetailRoom(roomId);
    },
    methods:{
        showImg(index) {
            this.index = index;
            this.visible = true;
        },
        handleHide() {
            this.visible = false;
        },
        GetDetailRoom(roomId){
            apiClient.get(`/Room/GetDetailFindRoom?roomId=${roomId}`)
                .then(response=>{
                    this.roomData = response.data;
                    console.log(this.roomData);
                })
                .catch(error=>{
                    this.message = "Lấy thông tin phòng bị lỗi: " + error.response?.data?.message || error;
                    this.snackbar = true;
                    this.snackbarColor = 'red';
                })
        },
        FormatPrice(price){
            return formatTablePrice (price);
        },
        GoToInfoZalo(){
            const url = 'https://zalo.me/' + this.roomData.sdtZalo;
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
        }
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
        <v-row class="m-4"> 
            <v-col cols="12" xxl="11">
                <pageheader title="" pageTitle="Chi tiết phòng trọ" />
                <BRow>
                    <BCol class="col-lg-8">
                        <BCard no-body class="mt-2">
                            <v-row justify="center" class="m-0" >
                                <!-- Carousel chính -->
                                <v-carousel v-model="currentSlide" hide-delimiters show-arrows="hover" height="480" class="item" >
                                    <v-carousel-item v-for="(item, index) in roomData.pathImgRoom" :key="index">
                                        <v-img :src="item" @click="showImg(index)" />
                                    </v-carousel-item>
                                </v-carousel>

                                <!-- Thumbnails -->
                                <v-row justify="center" class="m-0">
                                    <div
                                    v-for="(item, index) in roomData.pathImgRoom"
                                    :key="index"
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
                                <h4>{{ roomData.towerName }}</h4>
                                <p><v-icon>mdi-map-marker-radius</v-icon>{{ roomData.towerAddress }}</p>
                                <h3>Giá: <span style="color: red;">{{ roomData.priceRoom }}</span></h3>
                            </BCardHeader>
                            <BCardBody class="p-0 py-3">
                                <h4>Thông tin thiết bị</h4>
                                <p>Các thiết bị của phòng: {{ roomData.device }} </p>
                                <h4>Thông tin dịch vụ</h4>
                                <BRow>
                                    <BCol class="col-lg-4" v-for="(service,index) in roomData.service" :key="index">
                                        <p>+ {{ service.name }}: {{ FormatPrice(service.price) }}/{{ service.unitOfCalculation }}</p>
                                    </BCol>
                                </BRow>
                                <h4>Mô tả</h4>
                                <p>{{ moTa }}</p>
                            </BCardBody>
                        </BCard>
                    </BCol>
                    <BCol class="col-lg-4">
                        <BCard class="statistics-card-1" no-body>
                            <BCardBody class="text-center">
                                <div class="chat-avtar d-inline-flex mx-auto">
                                    <img class="rounded-circle img-fluid wid-90 img-thumbnail"
                                        :src="roomData.landlordAvata" alt="User image">
                                </div>
                                <h5 class="mb-0">{{ roomData.landlordName }}</h5>
                                <div class="d-grid gap-2 mt-3 ">
                                    <BButton class="customBtn"><v-icon class="mr-2">mdi-phone</v-icon>{{ roomData.sdtLandlord }}</BButton>
                                </div>
                                <div class="d-grid gap-2 mt-3">
                                    <BButton variant="primary" @click="GoToInfoZalo"><v-icon class="mr-2">mdi-chat-processing-outline</v-icon>Nhắn tin Zalo</BButton>
                                </div>
                                <div class="d-flex justify-space-evenly mt-3">
                                    <BButton variant="white" class="p-0"><v-icon class="mr-2">mdi-heart-outline</v-icon>Lưu phòng</BButton>
                                    <BButton variant="white" class="p-0" @click="copyCurrentURL"><v-icon class="mr-2">mdi-share-variant</v-icon>Chia sẻ phòng</BButton>
                                </div>
                            </BCardBody>
                        </BCard>
                    </BCol>
                </BRow>
            </v-col>
        </v-row>
    <vue-easy-lightbox :visible="visible" :imgs="roomData.pathImgRoom" :index="index" @hide="handleHide"></vue-easy-lightbox>
</template>