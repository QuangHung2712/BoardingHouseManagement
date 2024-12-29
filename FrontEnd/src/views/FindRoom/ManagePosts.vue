
<script>
    export default {
        data(){
            return{

            }
        }
    }
</script>
<template>
    <v-container>
        <h3>Danh sách các bài đăng</h3>
            <BCard no-body>
                <BCardBody class="p-0">
                    <BRow>
                        <BCol class="col-md-4 col-12">
                            <v-window style="height: 100%;"
                                v-model="window"
                                show-arrows
                                theme="dark"
                            >
                                <v-window-item
                                v-for="n in length"
                                :key="n"
                                >
                                <v-card class="d-flex justify-center align-center" height="200px">
                                    <v-img
                                        src="/images/Room/20241119180012-04a6_wm.jpg"
                                        alt="Room Image"
                                        aspect-ratio="16/9"
                                        class="rounded elevation-2"
                                        contain
                                    ></v-img>
                                </v-card>
                                </v-window-item>
                            </v-window>
                        </BCol>
                        <BCol class="col-md-8 col-12 mt-4">
                            <router-link to="/detail">
                                <h4> Số 11 Ngõ 91 đường Cầu Diễn </h4>
                                <h5>Giá: <span style="color: red;">3.000.000 VNĐ</span></h5>
                                <p><v-icon>mdi-map-marker-radius</v-icon> Số 11 Ngõ 91 đường Cầu Diễn Phường Cầu Diễn </p>
                                <p>Thiết bị: Máy giặt, Thang máy, Điều hoà, Nóng lạnh</p>
                            </router-link>
                            <div class="text-right">
                                <BButton variant="primary" class="mr-2">Đã tìm được</BButton>
                                <BButton variant="primary" class="mr-2"><v-icon >mdi-pencil</v-icon></BButton>
                                <BButton variant="primary" class="mr-2"><v-icon >mdi-delete</v-icon></BButton>
                            </div>
                        </BCol>
                    </BRow>
                </BCardBody>
            </BCard>
            <BCard no-body>
                <BCardBody class="p-0">
                    <BRow>
                        <BCol class="col-md-4 col-12">
                            <v-window style="height: 100%;"
                                v-model="window"
                                show-arrows
                                theme="dark"
                            >
                                <v-window-item
                                v-for="n in length"
                                :key="n"
                                >
                                <v-card class="d-flex justify-center align-center" height="200px">
                                    <v-img
                                        src="/images/Room/20241119180012-04a6_wm.jpg"
                                        alt="Room Image"
                                        aspect-ratio="16/9"
                                        class="rounded elevation-2"
                                        contain
                                    ></v-img>
                                </v-card>
                                </v-window-item>
                            </v-window>
                        </BCol>
                        <BCol class="col-md-8 col-12 mt-4">
                            <router-link to="/detail">
                                <h4> Số 11 Ngõ 91 đường Cầu Diễn </h4>
                                <h5>Giá: <span style="color: red;">3.000.000 VNĐ</span></h5>
                                <p><v-icon>mdi-map-marker-radius</v-icon> Số 11 Ngõ 91 đường Cầu Diễn Phường Cầu Diễn </p>
                                <p>Thiết bị: Máy giặt, Thang máy, Điều hoà, Nóng lạnh</p>
                            </router-link>
                            <div class="text-right">
                                <BButton variant="primary" class="mr-2">Đã tìm được</BButton>
                                <BButton variant="primary" class="mr-2"><v-icon >mdi-pencil</v-icon></BButton>
                                <BButton variant="primary" class="mr-2"><v-icon >mdi-delete</v-icon></BButton>
                            </div>
                        </BCol>
                    </BRow>
                </BCardBody>
            </BCard>
            <BCard no-body>
                <BCardBody class="p-0">
                    <BRow>
                        <BCol class="col-md-4 col-12">
                            <v-window style="height: 100%;"
                                v-model="window"
                                show-arrows
                                theme="dark"
                            >
                                <v-window-item
                                v-for="n in length"
                                :key="n"
                                >
                                <v-card class="d-flex justify-center align-center" height="200px">
                                    <v-img
                                        src="/images/Room/20241119180012-04a6_wm.jpg"
                                        alt="Room Image"
                                        aspect-ratio="16/9"
                                        class="rounded elevation-2"
                                        contain
                                    ></v-img>
                                </v-card>
                                </v-window-item>
                            </v-window>
                        </BCol>
                        <BCol class="col-md-8 col-12 mt-4">
                            <router-link to="/detail">
                                <h4> Số 11 Ngõ 91 đường Cầu Diễn </h4>
                                <h5>Giá: <span style="color: red;">3.000.000 VNĐ</span></h5>
                                <p><v-icon>mdi-map-marker-radius</v-icon> Số 11 Ngõ 91 đường Cầu Diễn Phường Cầu Diễn </p>
                                <p>Thiết bị: Máy giặt, Thang máy, Điều hoà, Nóng lạnh</p>
                            </router-link>
                            <div class="text-right">
                                <BButton variant="primary" class="mr-2">Đã tìm được</BButton>
                                <BButton variant="primary" class="mr-2"><v-icon >mdi-pencil</v-icon></BButton>
                                <BButton variant="primary" class="mr-2"><v-icon >mdi-delete</v-icon></BButton>
                            </div>
                        </BCol>
                    </BRow>
                </BCardBody>
            </BCard>
    </v-container>
</template>