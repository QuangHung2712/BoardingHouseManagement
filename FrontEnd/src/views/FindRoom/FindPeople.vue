<script>

import Axios from "axios";

export default {
    data() {
        return{
            tinhData: [],
            huyenData: [],
            phuongData: [],
            Gender: [
                {title: 'Nam',value: 0},
                {title: 'Nữ',value: 1},
                {title: 'Khác',value: 2},
            ],
            selectTinh: null,
            selectHuyen: null,
            selectPhuong: null,
            searchPrice: [0,10000000],
            status: false,
            length: 3,
            window: 0,
        }
    },
    name: "LANDING",
    components: {
        
    },
    created(){
        this.GetTinh();
    },
    methods: {
        GetTinh(){
            Axios.get(`https://esgoo.net/api-tinhthanh/1/0.htm`)
                .then(response=>{
                    this.tinhData =  response.data.data;
                })
                .catch(error=>{
                    this.message = "Lấy danh sách tỉnh thành bị lỗi " + error;
                    this.snackbar = true;
                    this.snackbarColor = 'red';
                })
        },
        onTinhChange(){
            Axios.get(`https://esgoo.net/api-tinhthanh/2/${this.selectTinh}.htm`)
                .then(response=>{
                    this.huyenData =  response.data.data;
                })
                .catch(error=>{
                    this.message = "Lấy danh sách tỉnh thành bị lỗi " + error;
                    this.snackbar = true;
                    this.snackbarColor = 'red';
                })
        },
        onHuyenChange(){
            Axios.get(`https://esgoo.net/api-tinhthanh/3/${this.selectHuyen}.htm`)
                .then(response=>{
                    this.phuongData =  response.data.data;
                })
                .catch(error=>{
                    this.message = "Lấy danh sách tỉnh thành bị lỗi " + error;
                    this.snackbar = true;
                    this.snackbarColor = 'red';
                })
        },
    },
}
</script>
<template>
    <v-container class="py-0">
            <BCard>
                <BRow>
                    <BCol class="col-xl-3 col-md-4 col-12">
                        <div class="form-group mt-lg-3">
                            <label class="form-label">Chọn tỉnh, thành phố</label>
                            <v-autocomplete
                                v-model="selectTinh"
                                :items="tinhData"
                                item-title="full_name"
                                item-value="id"
                                outlined
                                dense
                                clearable
                                @update:modelValue="onTinhChange"
                            ></v-autocomplete>
                        </div>
                    </BCol>
                    <BCol class="col-xl-3 col-md-4 col-12">
                        <div class="form-group mt-lg-3">
                            <label class="form-label">Quận, huyện</label>
                            <v-autocomplete
                                v-model="selectHuyen"
                                :items="huyenData"
                                item-title="full_name"
                                item-value="id"
                                outlined
                                dense
                                clearable
                                :disabled="!selectTinh"
                                @update:modelValue="onHuyenChange"
                            ></v-autocomplete>
                        </div>
                    </BCol>
                    <BCol class="col-xl-4 col-md-8">
                        <div class="form-group m-0">
                            <label class="form-label">Giá phòng</label>
                            <v-range-slider
                                v-model="searchPrice"
                                range
                                min="0"
                                max="10000000"
                                step="1000"
                                color="primary"
                                hide-details
                            ></v-range-slider>
                            <BRow>
                                <BCol class="col-6">
                                    <v-text-field
                                        label="Từ"
                                        v-model="searchPrice[0]"
                                        :type="'number'"
                                        hide-details
                                        suffix="VNĐ"
                                    ></v-text-field>
                                </BCol>
                                <BCol class="col-6">
                                    <v-text-field
                                        label="Đến"
                                        v-model="searchPrice[1]"
                                        :type="'text'"
                                        hide-details
                                        suffix="VNĐ"
                                    ></v-text-field>
                                </BCol>
                            </BRow>
                        </div>
                    </BCol>
                    <BCol class="col-xl-2 col-md-8">
                        <div class="form-group mt-lg-3">
                            <label class="form-label">Quận, huyện</label>
                            <v-select
                                clearable
                                label="Giới tính"
                                :items="Gender"
                                item-title="title"
                                item-value="value"
                                v-model="searchGender"
                                variant="outlined"
                                hide-details>
                            </v-select>
                        </div>
                    </BCol>
                    <BCol class="col-xl-12 col-md-4 col-12 d-flex justify-center align-center">
                        <v-btn class="mt-3 mt-md-0" prepend-icon="mdi-magnify">Tìm người ở ghép</v-btn>
                    </BCol>
                </BRow>
            </BCard>
        </v-container>

        <BContainerFluid class="container-fluid">
            <div class="mx-sm-3 ListRoom">
                <v-container class="pt-0">
                    <h3>Danh sách các bài đăng tìm người ở ghép</h3>
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
                                            <p>Giới tính: Nam</p>
                                        </router-link>
                                        <div class="text-right">
                                            <BButton variant="white"><v-icon>mdi-heart-outline</v-icon></BButton>
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
                                            <p>Giới tính: Nam</p>
                                        </router-link>
                                        <div class="text-right">
                                            <BButton variant="white"><v-icon>mdi-heart-outline</v-icon></BButton>
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
                                            <p>Giới tính: Nam</p>
                                        </router-link>
                                        <div class="text-right">
                                            <BButton variant="white"><v-icon>mdi-heart-outline</v-icon></BButton>
                                        </div>
                                    </BCol>
                                </BRow>
                            </BCardBody>
                        </BCard>
                </v-container>
            </div>

        </BContainerFluid>
</template>