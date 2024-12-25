<script>
import { Swiper, SwiperSlide } from 'swiper/vue';

import Axios from "axios";

export default {
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
        }
    },
    name: "LANDING",
    components: {
        Swiper,
        SwiperSlide,
        
    },
    created(){
        this.GetTinh();
    },
    methods: {
        changeMode(mode) {
            this.currentMode = mode;
            if (mode == "dark") {
                document.body.setAttribute("data-pc-theme", "dark");
                document.body.setAttribute("data-topbar", "dark");
                document.body.classList.remove("mode-auto");
            } else if (mode == "auto") {
                document.body.setAttribute("data-pc-theme", "light");
                document.body.setAttribute("data-topbar", "light");
                document.body.classList.add("mode-auto");
            } else {
                document.body.setAttribute("data-pc-theme", "light");
                document.body.setAttribute("data-topbar", "light");
                document.body.classList.remove("mode-auto");
            }
        },
        toggleMenu() {
            document.getElementById("navbarTogglerDemo01").classList.toggle("show");
        },
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
    <v-container class="pt-0">
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
                    <BCol class="col-xl-3 col-md-4 col-12 mt-lg-3">
                        <div class="form-group">
                            <label class="form-label">Phường, xã</label>
                            <v-autocomplete
                                v-model="selectPhuong"
                                :items="phuongData"
                                item-title="full_name"
                                item-value="id"
                                outlined
                                dense
                                clearable
                                :disabled="!selectHuyen"
                                multiple
                                @update:modelValue="onPhuongChange"
                            ></v-autocomplete>
                        </div>
                    </BCol>
                    <BCol class="col-xl-3 col-md-8">
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
                    <BCol class="col-xl-12 col-md-4 col-12 d-flex justify-center align-center">
                        <v-btn class="mt-3 mt-md-0" prepend-icon="mdi-magnify">Tìm phòng trọ</v-btn>
                    </BCol>
                </BRow>
            </BCard>
        </v-container>

        <BContainerFluid class="container-fluid">
            <!-- <div class="container-fluid"> -->
            <div class="bg-dark mx-sm-3 home-section " v-show="status">
               
                <div class="swiper language-slides-hero m-0">
                    <div class="swiper-wrapper " >
                        <swiper class="default-swiper rounded"  :autoplay="{
                            delay: 2500,
                            disableOnInteraction: false,
                        }">
                            <swiper-slide>
                                <BRow class="align-items-center justify-content-center text-center">
                                    <BCol sm="12" class="header-content">
                                        <BRow class="justify-content-center text-center">
                                            <BCol lg="8" sm="10" Cols="11" class="col-xl-7 col-md-9">
                                                <h1 class="my-3 wow animate__fadeInUp text-white" data-wow-delay="0.4s">Chào mừng bạn đến với trang web tìm kiếm nhà trọ số 1 Việt Nam.</h1>
                                            </BCol>
                                        </BRow>
                                        <BRow class="justify-content-center text-center">
                                            <BCol lg="7" sm="10" Cols="11" class="col-xxl-5 col-xl-6 col-md-8">
                                                <p class="f-16 mb-3 mb-lg-5 wow animate__fadeInUp" data-wow-delay="0.6s">Bạn vui lòng chọn địa chỉ trên thanh tìm kiếm để tìm phòng trọ phù hợp cho bạn.</p>
                                            </BCol>
                                        </BRow>
                                    </BCol>
                                </BRow>
                            </swiper-slide>
                        </swiper>
                    </div>
                </div>               
            </div>
            <div class="mx-sm-3 ListRoom">
                <v-container>
                        <BCard>
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
                                            <v-card class="d-flex justify-center align-center" height="250px">
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
                                    <BCol class="col-md-8 col-12">
                                        <router-link to="/detail">
                                            <h4> Số 11 Ngõ 91 đường Cầu Diễn </h4>
                                            <h5>Giá: <span style="color: red;">3.000.000 VNĐ</span></h5>
                                            <p><v-icon>mdi-map-marker-radius</v-icon> Số 11 Ngõ 91 đường Cầu Diễn Phường Cầu Diễn </p>
                                            <p>Thiết bị: Máy giặt, Thang máy, Điều hoà, Nóng lạnh</p>
                                            <BRow class="ml-1">
                                                <BCol class="col-xl-2 col-lg-4 col-6">-Điện: 100.000</BCol>
                                                <BCol class="col-xl-2 col-lg-4 col-6">-Điện: 100.000</BCol>
                                                <BCol class="col-xl-2 col-lg-4 col-6">-Máy giặt: 100.000</BCol>
                                                <BCol class="col-xl-2 col-lg-4 col-6">-Máy giặt: 100.000</BCol>
                                                <BCol class="col-xl-2 col-lg-4 col-6">-Máy giặt: 100.000</BCol>
                                                <BCol class="col-xl-2 col-lg-4 col-6">-Máy giặt: 100.000</BCol>
                                                
                                            </BRow>
                                        </router-link>
                                        <div class="text-right">
                                            <v-btn class="p-0"><v-icon>mdi-heart-outline</v-icon></v-btn>
                                        </div>
                                    </BCol>
                                </BRow>
                            </BCardBody>
                        </BCard>
                </v-container>
            </div>

        </BContainerFluid>
</template>