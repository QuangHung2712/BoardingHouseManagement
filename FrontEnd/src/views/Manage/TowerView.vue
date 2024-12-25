<style scoped>
    .slide-up-enter-active, .slide-up-leave-active {
        transition: transform 0.5s ease, opacity 0.5s ease;
    }

    .slide-up-enter, .slide-up-leave-to /* .slide-up-leave-active in <2.1.8 */ {
        transform: translateY(30px); /* Bắt đầu từ dưới lên */
        opacity: 0; /* Làm mờ khi biến mất */
    }

    .slide-up-enter-to, .slide-up-leave {
        transform: translateY(0); /* Vị trí ban đầu sau khi hoàn thành hiệu ứng */
        opacity: 1; /* Độ mờ bình thường khi hoàn thành hiệu ứng */
    }
    .m0{
        margin: 0px;
    }
    .p0{
        padding: 0px;
    }
    .itemTower{
        margin: 1%;
        border-radius: 25px;
        max-width: 400px;
    }
    .dialog{
        width: 80%;
        height: 80vh;
    }
    .dialog .v-btn{
        background-color: blue;
        color: whitesmoke;
    }
</style>
<script>
    import Navbar from "@/components/navbar.vue"
    import Rightbar from "@/components/right-bar.vue"
    import apiClient from "@/plugins/axios";
    import Axios from "axios";
    import CryptoJS from 'crypto-js';
    import Swal from "sweetalert2";
    export default {
        name: "HOMEPAGE",
        components: {
            Navbar, Rightbar,
        },
        data(){
            return{
                towerData: [
                    {id : 1,address: 'Số 11 ngõ 91',sumRoom: '11',roomRented:'5',roomStillEmpty: '5'},
                    {id : 2,address: 'Số 11 ngõ 91',sumRoom: '11',roomRented:'5',roomStillEmpty: '5'},
                    {id : 3,address: 'Số 11 ngõ 91',sumRoom: '11',roomRented:'5',roomStillEmpty: '5'},
                    {id : 4,address: 'Số 11 ngõ 91',sumRoom: '11',roomRented:'5',roomStillEmpty: '5'},
                ],
                landLordId: 1,
                message: '',
                snackbar: false,
                snackbarColor: '',
                viewdialogEdit: false,
                tower:{},
                tinhData: [
                    
                ],
                huyenData: [
                    
                ],
                phuongData: [
                    
                ],
                selectTinh: null,
                selectHuyen: null,
                selectPhuong: null,
                handleIconClick: false,
                address: '',
                form: false,
                titleDialog: '',
                searchAddress: '',
            }
        },
        computed: {
            isFixedWidth() {
                return this.$store.getters.isFixedWidth;
            },
                // Lọc danh sách tháp theo địa chỉ
            filteredTowers() {
                return this.towerData.filter(tower =>tower.address.toLowerCase().includes((this.searchAddress || '').toLowerCase()))
            },
        },
        created(){
            this.getTower();
        },
        methods:{
            required (v) {
                return !!v || 'Vui lòng không để trống'
            },
            goToTowerDetails(idtower) {
                //Mã hóa Id của tower
                const encryptedId = CryptoJS.enc.Base64.stringify(CryptoJS.enc.Utf8.parse(idtower));
                this.$router.push({ name: 'towerDetails', params: { idtower: encryptedId } });
            },
            getTower(){
                apiClient.get(`/Tower/GetAllTowerByLandLordId/${this.landLordId}`)
                        .then(response=>{
                            this.towerData = response.data;
                        })
                        .catch(error=>{
                            this.message = "Lấy danh sách tòa nhà bị lỗi " + error;
                            this.snackbar = true;
                            this.snackbarColor = 'red';
                        })
            },
            async btnCreateEdit(id){
                if(id != 0){
                    this.titleDialog = 'Sửa toà nhà';
                    const result = this.towerData.find(a => a.id === id);
                    const { towerName, address } = result;
                    // Gán các giá trị này vào this.tower
                    this.tower = { towerName, address };
                }
                else{
                    this.titleDialog = 'Thêm toà nhà';
                    this.form = false;
                    this.tower= {};
                }
                this.tower.id = id;
                await Axios.get(`https://esgoo.net/api-tinhthanh/1/0.htm`)
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
                this.tower.address = this.address + this.onPhuongChange();
            },
            CreateEditTower(){
                if(this.tower.id ===0){
                    this.message = "Thêm "
                }
                else{
                    this.message = "Sửa "
                }
                this.tower.landlordId = 1;
                apiClient.post(`/Tower/CreateEditTower`,this.tower)
                        .then(()=>{
                            this.message += "thành công"
                            this.snackbar = true;
                            this.snackbarColor = 'green';
                            this.viewdialogEdit = false;
                            this.getTower();
                        })
                        .catch(error =>{
                            this.message += "bị lỗi "+ error;
                            this.snackbar = true;
                            this.snackbarColor = 'red';
                        })
            },
            deleteTower(id,name){
                const swalWithBootstrapButtons = Swal.mixin({
                    customClass: {
                        confirmButton: "btn btn-success",
                        cancelButton: "btn btn-danger ml-2",
                    },
                    buttonsStyling: false,
                });

                swalWithBootstrapButtons
                    .fire({
                        title: "Bạn có chắc chắn không?",
                        text: `Bạn đang muốn xóa tòa nhà ở địa chỉ: ${name}`,
                        icon: "warning",
                        confirmButtonText: "Có!",
                        cancelButtonText: "Không!",
                        showCancelButton: true,
                    })
                    .then((confirm) => {
                        if (confirm.value) {
                                apiClient.delete(`/Tower/DeleteTower/${id}`)
                                        .then(reponse=> {
                                            console.log(reponse)
                                            if(reponse.status){
                                                swalWithBootstrapButtons.fire(
                                                "Xóa thành công!",
                                                `Đã xóa thành công tòa nhà ở địa chỉ: ${name}`,
                                                "success")
                                                this.getTower();
                                            }
                                            else{
                                                swalWithBootstrapButtons.fire(
                                                    "Xóa không thành công",
                                                    `Đã xảy ra lỗi khi xóa tòa nhà ở địa chỉ: ${name}`,
                                                    "error"
                                                );
                                            }
                                        })
                                        .catch(error =>{
                                            swalWithBootstrapButtons.fire(
                                                "Xóa không thành công",
                                                ` ${error}`,
                                                "error"
                                            );
                                        })
                        } else if ( /* Read more about handling dismissals below */ confirm.dismiss === Swal.DismissReason.cancel) return
                    });
            }
            
        }
}
</script>

<template>
    <div class="pc-header" style="z-index: 1;">
        <Navbar />
    </div>

    <div class="pc-container m-0">
        <v-container >
            <div class="page-header">
                <div class="page-block">
                    <BRow class="align-items-center">
                        <BCol md="12">
                            <div class="page-header-title">
                                <h2 class="mb-0">Toà nhà</h2>
                            </div>
                        </BCol>
                    </BRow>
                </div>
            </div>
            <!-- Start Content-->
            <v-snackbar
                v-model="snackbar"
                :timeout="10000"
                class="custom-snackbar"
                :color="snackbarColor"
            >
                <h5 class="text-center">{{ message }}</h5>
            </v-snackbar>
            <v-card class="pa-4">
                <BRow>
                <BCol class="col-md-4">
                    <v-text-field label="Địa chỉ" variant="outlined" clearable hide-details v-model="searchAddress"></v-text-field>
                </BCol>
                <Bcol class="col-md-4">

                </Bcol>
                <Bcol class="col-md-4">
                    <v-btn color="blue-lighten-1" class="mt-2" @click="(viewdialogEdit = !viewdialogEdit) && (btnCreateEdit(0))"> Thêm toà nhà</v-btn>
                </Bcol>
            </BRow>
            </v-card>
            <v-card class="mt-5">
                <div class="d-flex flex-wrap">
                    <v-card class="pa-4 itemTower" color="peach" dark v-for="item in filteredTowers" :key="item.id">
                        <v-card-title>
                            {{ item.towerName }}
                        </v-card-title>
                        <v-card-text>
                            <v-icon>mdi-map-marker</v-icon>
                            <span>{{ item.address }}</span>
                            <p class="mt-2">Số phòng: {{item.sumRoom}}</p>
                            <p class="mt-2">Đã cho thuê: {{ item.roomRented }}</p>
                            <p  class="mt-2">Còn trống: {{ item.roomStillEmpty }}</p>
                            <v-row justify="space-around" class="mt-2">
                                <v-btn icon size="small" @click="deleteTower(item.id,item.address)">
                                    <v-icon>mdi-delete</v-icon>
                                </v-btn>
                                <v-btn icon size="small" @click="(viewdialogEdit = !viewdialogEdit) && (btnCreateEdit(item.id))">
                                    <v-icon>mdi-pencil</v-icon>
                                </v-btn>
                                <v-btn @click="goToTowerDetails(item.id)" icon size="small">
                                    <v-icon>mdi-eye</v-icon>
                                </v-btn>
                            </v-row>
                        </v-card-text>
                    </v-card>
                </div>
            </v-card>
        <BModal v-model="viewdialogEdit" hide-footer :title="titleDialog" modal-class="fadeInRight"
                class="v-modal-custom" centered size="lg" >
                <div class="card-body">
                    <v-form v-model="form" ref="form">
                        <BRow>
                            <BCol class="col-lg-12">
                                <div class="form-group">
                                    <label class="form-label">Tên tòa nhà:</label>
                                    <v-text-field v-model="tower.towerName" :rules="[required]" variant="outlined" clearable placeholder="Nhập vào tên dịch vụ" class="input-control"></v-text-field>
                                </div>
                            </BCol>
                            <div v-show="handleIconClick" v-cloak>
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
                                    </BRow>
                                </transition>
                            </div>
                            <BCol class="col-lg-12">
                                <div class="form-group">
                                    <label class="form-label">Địa chỉ:</label>
                                    <v-text-field v-model="tower.address" variant="outlined" :rules="[required]" readonly class="input-control" append-icon="mdi-map-marker-radius" @click="handleIconClick = !handleIconClick"></v-text-field>
                                </div>
                            </BCol>
                        </BRow>
                    </v-form>
                </div>
                <div class="modal-footer v-modal-footer">
                    <BButton type="button" variant="light" @click="viewdialogEdit = false">Close
                    </BButton>
                    <BButton type="button" variant="primary" @click="CreateEditTower()" :disabled="!form">Save Changes</BButton>
                </div>
            </BModal>
            <div>
                <slot />
            </div>
        </v-container>
    </div>
    <div class="pc-footer">
        
    </div>
    <Rightbar />
</template>