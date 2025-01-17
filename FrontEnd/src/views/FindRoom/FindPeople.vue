<style scoped>
    .saved-button {
        color: red !important; /* Màu chữ trắng */
    }
</style>
<script>
    import Axios from "axios";
    import store from "../../state/store";
    import apiClient from '@/plugins/axios';
    import common from "@/components/common/JavaScripCommon"

    export default {
        data() {
            return{
                tinhData: [],
                huyenData: [],
                Gender: [
                    {title: 'Nam',value: 1},
                    {title: 'Nữ',value: 2},
                ],
                selectTinh: null,
                selectHuyen: null,
                searchPrice: [0,10000000],
                status: false,
                message: '',
                snackbar: false,
                snackbarColor: '',
                customerId: 0,
                indexSelect: 0,
                searchGender: null,
                PostData:[],
            }
        },
        name: "LANDING",
        components: {
            
        },
        created(){
            this.customerId = store.getters['getCustomerId'] ?? 0;
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
            FormatPrice(price){
                return common.formatTablePrice(price);
            },
            FindPeople(){
                const tinh = this.tinhData.find(item => item.id === this.selectTinh);
                const huyen = this.huyenData.find(item => item.id === this.selectHuyen);
                var searchaddress ='';
                
                if (tinh && huyen) {
                    searchaddress = ` ${huyen.full_name} ${tinh.full_name}`;
                }
                else{
                    this.message = "Vui lòng chọn đầy đủ  địa chỉ trước ";
                    this.snackbar = true;
                    this.snackbarColor = 'red';
                    return;
                }
                const priceArrive = this.searchPrice[1]
                apiClient.get(`/Post/SearchPost`,{
                    params: {
                        address: searchaddress,
                        priceFrom: this.searchPrice[0],
                        priceArrive: priceArrive,
                        customerId: this.customerId ?? 0,
                        gender : this.searchGender ?? 0,
                    },
                })
                        .then(response=>{
                            this.PostData = response.data
                            this.status = true;
                        })
                        .catch(error=>{
                            this.message = "Lấy danh sách bị lỗi: " + error.response?.data?.message || error;
                            this.snackbar = true;
                            this.snackbarColor = 'red';
                        })
            },
            GotoDetail(postId){
                const route = this.$router.resolve({ name: 'detailpost', params: { idpost: postId } });
                window.open(route.href, '_blank');
            },
            SavePost(postId,status,index){
                this.customerId = store.getters['getCustomerId'] ?? 0;
                // Hiển thị thông báo dựa trên trạng thái
                if (status) {
                this.message = "Xoá phòng thành công";
                } else {
                this.message = "Lưu phòng thành công";
                }

                // Đặt trạng thái đang xử lý
                this.PostData[index].isProcessing = true;

                // Gửi yêu cầu API
                apiClient
                .put(`/Post/SavePost?customerId=${this.customerId}&postId=${postId}&status=${status}`)
                .then(() => {
                    // Cập nhật trạng thái `isSave` sau khi API trả về thành công
                    this.PostData[index].isSave = !this.PostData[index].isSave;

                    // Hiển thị thông báo thành công
                    this.snackbar = true;
                    this.snackbarColor = "green";
                    this.indexSelect = index;
                })
                .catch((error) => {
                    // Xử lý lỗi
                    this.message =
                    "Lưu phòng đã xảy ra lỗi: " +
                    (error.response?.data?.message || error.message || "Không xác định");
                    this.snackbar = true;
                    this.snackbarColor = "red";
                })
                .finally(() => {
                    // Đặt lại trạng thái xử lý
                    this.PostData[index].isProcessing = false;
                });
            },
        },
    }
</script>
<template>
    <v-container class="py-0">
        <v-snackbar
            v-model="snackbar"
            :timeout="10000"
            class="custom-snackbar"
            :color="snackbarColor"
        >
            <h5 class="text-center">{{ message }}</h5>
        </v-snackbar>
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
                            <label class="form-label">Giới tính</label>
                            <v-select
                                clearable
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
                        <v-btn class="mt-3 mt-md-0" prepend-icon="mdi-magnify" @click="FindPeople()">Tìm người ở ghép</v-btn>
                    </BCol>
                </BRow>
            </BCard>
        </v-container>

        <BContainerFluid class="container-fluid">
            <div class="bg-dark mx-sm-3 home-section justify-content-center" v-if="!status">
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
                                               <h1 class="my-3 wow animate__fadeInUp text-white" data-wow-delay="0.4s">Chào mừng bạn đến với trang web tìm kiếm người ở ghép.</h1>
                                           </BCol>
                                       </BRow>
                                       <BRow class="justify-content-center text-center">
                                           <BCol lg="7" sm="10" Cols="11" class="col-xxl-5 col-xl-6 col-md-8">
                                               <p class="f-16 mb-3 mb-lg-5 wow animate__fadeInUp" data-wow-delay="0.6s">Bạn vui lòng chọn địa chỉ trên thanh tìm kiếm để tìm người ở ghép phù hợp cho bạn.</p>
                                           </BCol>
                                       </BRow>
                                   </BCol>
                               </BRow>
                           </swiper-slide>
                       </swiper>
                   </div>
               </div>               
           </div>
            <div class="mx-sm-3" v-else>
                <v-container class="pt-0">
                    <v-row>
                        <v-col md="2"></v-col>
                        <h1 v-if="postData == 0">Bạn không có bài đăng nào</h1>
                        <v-col cols="12" md="8" v-else>
                            <h3>Danh sách các bài đăng tìm người ở ghép</h3>
                            <BCard no-body v-for="(post, index) in PostData" :key="index">
                                <BCardBody class="p-0">
                                    <BRow>
                                        <BCol class="col-md-4 col-12">
                                            <v-window style="height: 100%;"
                                                v-model="post.selectimg"
                                                show-arrows
                                                theme="dark"
                                                v-if="post.img != 0"
                                            >
                                                <v-window-item
                                                v-for="(item,index) in post.img"
                                                :key="index"
                                                >
                                                <v-card class="d-flex justify-center align-center" height="200px">
                                                    <v-img
                                                        :src="item"
                                                        alt="Post Image"
                                                        aspect-ratio="16/9"
                                                        class="rounded elevation-2"
                                                        contain
                                                    ></v-img>
                                                </v-card>
                                                </v-window-item>
                                            </v-window>
                                            <h4 v-else class="text-center">Không có ảnh</h4>
                                        </BCol>
                                        <BCol class="col-md-8 col-12 mt-4">
                                            <router-link @click="GotoDetail(post.id)" target="_blank">
                                                <h4> {{ post.name }}</h4>
                                                <h5>Giá: <span style="color: red;">{{ FormatPrice(post.price) }}</span></h5>
                                                <p><v-icon>mdi-map-marker-radius</v-icon> {{ post.address }} </p>
                                                <p>Giới tính: {{ post.gender }}</p>
                                            </router-link>
                                            <div class="text-right">
                                                <BButton variant="white" :class="{'saved-button': post.isSave}" @click="SavePost(post.id,post.isSave,index)">
                                                   <v-icon v-if="!post.isProcessing">mdi-heart</v-icon>
                                                    <v-progress-circular
                                                    v-else
                                                    indeterminate
                                                    color="white"
                                                    size="20"
                                                    ></v-progress-circular>
                                                </BButton>
                                            </div>
                                        </BCol>
                                    </BRow>
                                </BCardBody>
                            </BCard>
                        </v-col>
                    </v-row>
                </v-container>
            </div>

        </BContainerFluid>
</template>