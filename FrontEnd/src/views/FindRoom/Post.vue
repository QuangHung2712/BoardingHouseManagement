<script>
    import store from "../../state/store";
    import Axios from "axios";
    import common from "@/components/common/JavaScripCommon"
    import CryptoJS from 'crypto-js';
    import apiClient from "@/plugins/axios";

    export default {
        data(){
            return{
                rules: {
                    required: v => !!v || 'Vui lòng không để trống',
                },
                Gender:[
                    {title: 'Nam', value: 1},
                    {title: 'Nữ', value: 2}
                ],
                post:{

                },
                tinhData: [
                    
                ],
                huyenData: [
                    
                ],
                phuongData: [
                    
                ],
                selectTinh: null,
                selectHuyen: null,
                selectPhuong: null,
                address: '',
                addressConfirm: '',
                previewUrls: [],
                viewDiaLogAddress:false,
                message: '',
                snackbar: false,
                snackbarColor: '',
                form: false,
            }
        },
        created(){
            const idpost = this.$route.params.idpost;
            this.post.id = CryptoJS.enc.Utf8.stringify(CryptoJS.enc.Base64.parse(idpost));
            if(this.post.id != 0){
                apiClient.get(`/Post/GetDetail/${this.post.id}`)
                        .then(response=>{
                            this.post = response.data;
                            this.loadImages();
                            
                        })
                        .catch(error=>{
                            this.message = `Đã xảy ra lỗi: ${error.response?.data?.message || error.message}`;
                            this.snackbarColor = 'red';
                            this.snackbar = true;
                        })
            }
        },
        methods:{
            btnAddress(index){
                Axios.get(`https://esgoo.net/api-tinhthanh/1/0.htm`)
                    .then(response=>{
                        this.tinhData =  response.data.data;
                    })
                    .catch(error=>{
                        this.message = "Lấy danh sách tỉnh thành bị lỗi " + error;
                        this.snackbar = true;
                        this.snackbarColor = 'red';
                    })
                    this.indexAddress = index;
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
            onPhuongChange(){
                const tinh = this.tinhData.find(item => item.id === this.selectTinh);
                const huyen = this.huyenData.find(item => item.id === this.selectHuyen);
                const phuong = this.phuongData.find(item => item.id === this.selectPhuong);
                if (tinh && huyen && phuong) {
                    return ` ${phuong.full_name} ${huyen.full_name} ${tinh.full_name}`;
                } else {
                    return ''; // Trả về chuỗi rỗng nếu có một trong các biến là null hoặc undefined
                }
            },
            FormatAddress(){
                this.addressConfirm = this.address + this.onPhuongChange();
            },
            btnSaveAddress(){
                this.post.address = this.addressConfirm;
                this.viewDiaLogAddress = false;
            },
            FormatPrice(){
                this.post.priceRoom =  common.formatPrice(this.post.priceRoom );
            },
            async loadImages(){
                this.post.imgRoom = [];
                for (let url of this.post.pathImgRoom) {
                    try {
                    
                        const response = await fetch(url);
                        
                        const blob = await response.blob(); // Chuyển phản hồi thành Blob
                        const file = new File([blob], url.split('/').pop(), { type: blob.type }); // Tạo File từ Blob
                        // Lưu vào mảng imgRoom
                    this.post.imgRoom.push(file);
                        console.log(`Ảnh từ ${url} đã được tải và lưu vào imgRoom.`);
                    } catch (error) {
                        console.error(`Lỗi khi tải ảnh từ ${url}:`, error);
                    }
                }
                console.log('Tất cả ảnh đã được tải:', this.post.imgRoom);
            },
            handleFileChange() {
                this.previewUrls = [];
                if (this.post.imgRoom) {
                    for (let file of this.post.imgRoom) {
                    const reader = new FileReader();
                    reader.onload = (e) => {
                        this.previewUrls.push(e.target.result);
                    };
                    reader.readAsDataURL(file);
                    }
                    console.log(this.previewUrls)
                }
            },
            CreateEditPost(){
                const customerId = store.getters['getCustomerId'];
                console.log(this.post.id);
                if(this.post.id == 0){
                    this.message = 'Thêm thành công'
                }
                else{
                    this.message = 'Sửa thành công'
                }
                if(!/^\d+$/.test(this.post.priceRoom))
                this.post.priceRoom =  this.post.priceRoom.replace(/[^\d]/g, '');
                const formData = new FormData();
                formData.append("id",this.post.id);
                formData.append("name", this.post.name);
                formData.append("address", this.post.address);
                formData.append("gender",this.post.gender);
                formData.append("priceRoom", this.post.priceRoom);
                formData.append("describe",this.post.describe);
                formData.append("customerId", customerId);
                if(this.post.imgRoom)
                {
                    this.post.imgRoom.forEach((File)=>{
                        formData.append("ImgRoom", File);
                    })
                }
                apiClient.put(`/Post/CreateEdit`,formData)
                .then(() => {
                    this.snackbarColor = 'green'
                    this.snackbar = true
                    this.$router.push({ name: 'listPost'});
                })
                .catch(error =>{
                    this.message = 'Đã xảy ra lỗi ' + error.response?.data?.message || error
                    this.snackbarColor = 'red'
                    this.snackbar = true
                })
            }
        }
    }
</script>
<template>
    <v-container>
        <v-snackbar
            v-model="snackbar"
            :timeout="10000"
            class="custom-snackbar"
            :color="snackbarColor"
        >
            <h5 class="text-center">{{ message }}</h5>
        </v-snackbar>
        <v-row>
            <v-col md="2"></v-col>
            <v-col cols="12" md="8">
                <BCard>
                    <BCardHeader>
                        <h3>Đăng bài tìm người ở Ghép</h3>
                    </BCardHeader>
                    <BCardBody>
                        <v-form v-model="form">
                            <BRow>
                                <BCol class="col-lg-12">
                                    <div class="form-group mt-lg-3">
                                        <label class="form-label">Tiêu đề bài đăng</label>
                                        <v-text-field 
                                            type="text" 
                                            clearable 
                                            variant="outlined" 
                                            placeholder="Nhập vào tiêu đề bài đăng" 
                                            :rules="[rules.required]" 
                                            v-model="post.name"
                                        ></v-text-field>     
                                    </div>
                                </BCol>
                                <BCol class="col-lg-12">
                                    <div class="form-group">
                                        <label class="form-label">Địa chỉ:</label>
                                        <v-text-field v-model="post.address" :rules="[rules.required]" type="text" @click="(viewDiaLogAddress = !viewDiaLogAddress) && btnAddress(index)"  readonly variant="outlined" class="input-control"></v-text-field>
                                    </div>
                                </BCol>
                                <BCol class="col-lg-6">
                                    <div class="form-group">
                                        <label class="form-label">Giá phòng:</label>
                                        <v-text-field v-model="post.priceRoom" clearable :rules="[rules.required]" @input="FormatPrice" type="text" variant="outlined" class="input-control"></v-text-field>
                                    </div>
                                </BCol>
                                <BCol class="col-lg-6">
                                    <div class="form-group">
                                        <label class="form-label">Giới tính:</label>
                                        <v-select
                                            clearable
                                            :items="Gender"
                                            :rules="[rules.required]"
                                            item-title="title"
                                            item-value="value"
                                            v-model="post.gender"
                                            variant="outlined"
                                            >
                                        </v-select>
                                    </div>
                                </BCol>
                                <BCol class="col-lg-12">
                                    <div class="form-group">
                                        <label class="form-label">Mô tả:</label>
                                        <v-textarea v-model="post.describe" clearable :rules="[rules.required]" variant="outlined"></v-textarea>
                                    </div>
                                </BCol>
                                <BCol class="col-lg-12">
                                    <div class="form-group">
                                        <label class="form-label">Ảnh phòng:</label>
                                        <v-file-input
                                            v-model="post.imgRoom"
                                            multiple
                                            accept="image/*"
                                            show-size
                                            @change="handleFileChange"
                                        ></v-file-input>
                                    </div>
                                </BCol>
                                <v-row>
                                    <v-col
                                        v-for="(file, index) in post.imgRoom"
                                        :key="index"
                                        cols="12"
                                        sm="6"
                                        md="3"
                                    >
                                        <v-img
                                            :src="previewUrls?.[index] || post?.pathImgRoom?.[index]"
                                            aspect-ratio="1"
                                            class="mb-4"
                                        ></v-img>
                                        <p>{{ file.name }}</p>
                                    </v-col>
                                </v-row>
                            </BRow>
                        </v-form>
                    </BCardBody>
                    <BCardFooter>
                        <BButton type="button" variant="primary" @click="CreateEditPost()" :disabled="!form">Save Changes</BButton>
                    </BCardFooter>
                </BCard>
            </v-col>
        </v-row>
        <BModal v-model="viewDiaLogAddress" hide-footer title="Chọn địa chỉ" modal-class="fadeInRight"
            class="v-modal-custom" centered size="lg" >
            <div class="card-body">
                <v-form v-model="formAddress" ref="formChange">
                    <transition name="slide-up">
                        <BRow>
                            <BCol class="col-lg-4">
                                <div class="form-group">
                                    <label class="form-label">Tỉnh</label>
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
                            <BCol class="col-lg-4">
                                <div class="form-group">
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
                            <BCol class="col-lg-4">
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
                                        @update:modelValue="onPhuongChange"
                                    ></v-autocomplete>
                                </div>
                            </BCol>
                            <BCol class="col-lg-12">
                                <div class="form-group">
                                <label class="form-label">Số nhà:</label>
                                <v-text-field
                                    v-model="address"
                                    variant="outlined"
                                    :disabled="!selectPhuong"
                                    clearable
                                    placeholder="Nhập vào tên dịch vụ"
                                    class="input-control"
                                    @input="FormatAddress"
                                ></v-text-field>
                                </div>
                            </BCol>
                            <BCol class="col-lg-12">
                                <div class="form-group">
                                <label class="form-label">Địa chỉ:</label>
                                <v-text-field
                                    v-model="addressConfirm"
                                    variant="outlined"
                                    readonly
                                    class="input-control"
                                    :rules="[required]"
                                ></v-text-field>
                                </div>
                            </BCol>
                        </BRow>
                    </transition>
                </v-form>
            </div>
            <div class="modal-footer v-modal-footer">
                <BButton type="button" variant="light" @click="viewDiaLogAddress = false">Close
                </BButton>
                <BButton type="button" variant="primary" @click="btnSaveAddress()" :disabled="!formAddress">Save Changes</BButton>
            </div>
        </BModal>
    </v-container>
</template>