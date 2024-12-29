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
    import VueEasyLightbox from "vue-easy-lightbox";

    export default {
        name: "LANDING",
        components: {
            pageheader,VueEasyLightbox
        },
    data() {
        return{
            tinhData: [],
            huyenData: [],
            phuongData: [],
            selectTinh: null,
            selectHuyen: null,
            selectPhuong: null,
            searchPrice: [0,10000000],
            status: false,
            length: 3,
            window: 0,
            currentSlide: 0,
            visible: false,
            index: 0,
            images: [
                { src: "/images/Room/20241119180011-5e94_wm.jpg", name: "Tên ảnh 1" },
                { src: "/images/Room/20241119180011-9dc9_wm.jpg", name: "Tên ảnh 2" },
                { src: "/images/Room/20241119180011-d4ae_wm.jpg", name: "Tên ảnh 3" },
                { src: "/images/Room/20241119180011-e1cc_wm.jpg", name: "Tên ảnh 3" },
                
            ],
        }
    },
    methods:{
        showImg(index) {
            this.index = index;
            this.visible = true;
        },
        handleHide() {
            this.visible = false;
        },
    }
}
</script>
<template>
    <v-container>

        <v-row>
            <v-col offset="1" cols="11">
                <pageheader title="" pageTitle="Chi tiết phòng trọ" />
                <BRow>
                    <BCol class="col-lg-8">
                        <BCard no-body class="mt-2">
                            <v-row justify="center" class="m-0" >
                                <!-- Carousel chính -->
                                <v-carousel v-model="currentSlide" hide-delimiters show-arrows="hover" height="480" class="item" >
                                    <v-carousel-item v-for="(item, index) in images" :key="index">
                                        <v-img :src="item.src" :alt="item.name" @click="showImg(index)" />
                                    </v-carousel-item>
                                </v-carousel>

                                <!-- Thumbnails -->
                                <v-row justify="center" class="m-0">
                                    <div
                                    v-for="(item, index) in images"
                                    :key="index"
                                    @click="currentSlide = index"
                                    class="thumbnail-container"
                                    >
                                    <v-img
                                        :src="item.src"
                                        :alt="item.name"
                                        class="thumbnail"
                                        :class="{ active: currentSlide === index }"
                                    />
                                    </div>
                                </v-row>
                            </v-row>
                        </BCard>
                        <BCard>
                            <BCardHeader >
                                <h4>Số 11 Ngõ 91 đường Cầu Diễn</h4>
                                <p><v-icon>mdi-map-marker-radius</v-icon> Số 11 Ngõ 91 Đường Cầu Diễn Phường Cầu Diễn Quận Nam Từ Liêm Thành phố Hà Nội </p>
                                <h3>Giá: <span style="color: red;">3.000.000 VNĐ</span></h3>
                            </BCardHeader>
                            <BCardBody class="p-0 py-3">
                                <h4>Thông tin thiết bị</h4>
                                <p>Các thiết bị của phòng: Máy giặt, điều hòa, </p>
                                <h4>Mô tả</h4>
                                <p>Các thiết bị của phòng: Máy giặt, điều hòa, </p>
                            </BCardBody>
                        </BCard>
                    </BCol>
                    <BCol class="col-lg-4">
                        <BCard class="statistics-card-1" no-body>
                            <BCardBody class="text-center">
                                <div class="chat-avtar d-inline-flex mx-auto">
                                    <img class="rounded-circle img-fluid wid-90 img-thumbnail"
                                        src="/images/Room/20241119180011-e1cc_wm.jpg" alt="User image">
                                </div>
                                <h5 class="mb-0">William Bond</h5>
                                <div class="d-grid gap-2 mt-3 ">
                                    <BButton class="customBtn"><v-icon class="mr-2">mdi-phone</v-icon>0359988934</BButton>
                                </div>
                                <div class="d-grid gap-2 mt-3">
                                    <BButton variant="primary"><v-icon class="mr-2">mdi-chat-processing-outline</v-icon>Nhắn tin Zalo</BButton>
                                </div>
                                <div class="d-flex justify-space-evenly mt-3">
                                    <BButton variant="white" class="p-0"><v-icon class="mr-2">mdi-heart-outline</v-icon>Lưu phòng</BButton>
                                    <BButton variant="white" class="p-0"><v-icon class="mr-2">mdi-share-variant</v-icon>Chia sẻ phòng</BButton>
                                </div>
                            </BCardBody>
                        </BCard>
                    </BCol>
                </BRow>
            </v-col>
        </v-row>
    </v-container>
    <vue-easy-lightbox :visible="visible" :imgs="images" :index="index" @hide="handleHide"></vue-easy-lightbox>
</template>