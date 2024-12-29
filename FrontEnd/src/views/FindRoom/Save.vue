<style scoped>
    p{
        font-size: 14px !important;
    }
    .v-window__left, .v-window__right {
        font-size: 16px; /* Điều chỉnh kích thước của nút */
        width: 10px !important; /* Thu nhỏ chiều rộng của nút */
        height: 30px; /* Thu nhỏ chiều cao của nút */
    }
</style>

<script>
    export default {
        data(){
            return{
                window: 0,
                length: 3,
            }
        }
    }
</script>
<template>
    <v-container>
        <v-row>
            <v-col cols="6">
                <BCard no-body>
                    <BCardHeader class="p-3">
                        <h4 class="m-0">Phòng đã lưu</h4>
                    </BCardHeader>
                    <BCardBody class="p-2">
                        <BCard no-body>
                            <BRow>
                                <BCol class="col-md-4 col-12">
                                    <v-img
                                        src="/images/Room/20241119180012-04a6_wm.jpg"
                                        alt="Room Image"
                                        aspect-ratio="16/9"
                                        class="rounded elevation-2"
                                        contain
                                    ></v-img>
                                </BCol>
                                <BCol class="col-md-8 col-12 mt-2">
                                    <router-link to="/detail">
                                        <h5> Số 11 Ngõ 91 đường Cầu Diễn </h5>
                                        <h6>Giá: <span style="color: red;">3.000.000 VNĐ</span></h6>
                                        <p><v-icon>mdi-map-marker-radius</v-icon> Số 11 Ngõ 91 đường Cầu Diễn Phường Cầu Diễn </p>
                                        <p>Thiết bị: Máy giặt, Thang máy, Điều hoà, Nóng lạnh</p>
                                    </router-link>
                                    <div class="text-right">
                                        <BButton variant="white"><v-icon>mdi-heart-outline</v-icon></BButton>
                                    </div>
                                </BCol>
                            </BRow>
                        </BCard>
                        <BCard no-body>
                            <BRow>
                                <BCol class="col-md-4 col-12">
                                    <v-img
                                        src="/images/Room/20241119180012-04a6_wm.jpg"
                                        alt="Room Image"
                                        aspect-ratio="16/9"
                                        class="rounded elevation-2"
                                        contain
                                    ></v-img>
                                </BCol>
                                <BCol class="col-md-8 col-12 mt-2">
                                    <router-link to="/detail">
                                        <h5> Số 11 Ngõ 91 đường Cầu Diễn </h5>
                                        <h6>Giá: <span style="color: red;">3.000.000 VNĐ</span></h6>
                                        <p><v-icon>mdi-map-marker-radius</v-icon> Số 11 Ngõ 91 đường Cầu Diễn Phường Cầu Diễn </p>
                                        <p>Thiết bị: Máy giặt, Thang máy, Điều hoà, Nóng lạnh</p>
                                    </router-link>
                                    <div class="text-right">
                                        <BButton variant="white"><v-icon>mdi-heart-outline</v-icon></BButton>
                                    </div>
                                </BCol>
                            </BRow>
                        </BCard>
                        <BCard no-body>
                            <BRow>
                                <BCol class="col-md-4 col-12">
                                    <v-img
                                        src="/images/Room/20241119180012-04a6_wm.jpg"
                                        alt="Room Image"
                                        aspect-ratio="16/9"
                                        class="rounded elevation-2"
                                        contain
                                    ></v-img>
                                </BCol>
                                <BCol class="col-md-8 col-12 mt-2">
                                    <router-link to="/detail">
                                        <h5> Số 11 Ngõ 91 đường Cầu Diễn </h5>
                                        <h6>Giá: <span style="color: red;">3.000.000 VNĐ</span></h6>
                                        <p><v-icon>mdi-map-marker-radius</v-icon> Số 11 Ngõ 91 đường Cầu Diễn Phường Cầu Diễn </p>
                                        <p>Thiết bị: Máy giặt, Thang máy, Điều hoà, Nóng lạnh</p>
                                    </router-link>
                                    <div class="text-right">
                                        <BButton variant="white"><v-icon>mdi-heart-outline</v-icon></BButton>
                                    </div>
                                </BCol>
                            </BRow>
                        </BCard>
                    </BCardBody>
                </BCard>
            </v-col>
            <v-col cols="6">
                <BCard no-body>
                    <BCardHeader class="p-3">
                        <h4 class="m-0">Tin đã lưu</h4>
                    </BCardHeader>
                    <BCardBody class="p-2">
                       <h2>Không có tin nào</h2>
                    </BCardBody>
                </BCard>
                
            </v-col>
        </v-row>
    </v-container>
</template>