<script>
import pageheader from "@/components/page-header.vue"
import apiClient from "@/plugins/axios";
import CryptoJS from 'crypto-js';
import store from "../../../../state/store";
import Axios from "axios";


export default {
    name: "wizard",
    components: {
        pageheader
    },
    data(){
        return{
            towerid: 0,
            selectRoom: [],
            servicedata: [],
            contractId: 0,
            titlePage: "",
            message: '',
            snackbar: false,
            snackbarColor: '',
            form1: false,
            form2: false,
            form3: false,
            IsRoom : false,
            isRepresentativeSelected: false,
            contractData: {
                id: 0,
                customers: [
                    {
                        id:0,
                        fullName: "",
                        doB: null,
                        phoneNumber: "",
                        email: null,
                        cCCD: "",
                        address: "",
                        isRepresentative : false,
                    },
                ],
                services:[
                    {
                        serviceId: null,
                        price: '',
                        number: 1,
                        isOldNewNumber: false,
                        currentNumber: 0, 
                    },
                ],
            },
            formAddress: false,
            viewDiaLogAddress: false,
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
            indexAddress: 0,

        }
    },
    created(){
        const path = this.$route.path;
        // Sử dụng regex để lấy `idtower` từ URL
        const match = path.match(/^\/([^/]+)/); // Tìm phần `/:idtower`
        const idtower = match ? match[1] : null;
        if (idtower) {
            this.towerid = CryptoJS.enc.Utf8.stringify(
                CryptoJS.enc.Base64.parse(idtower)
            );
        } else {
            console.error("Không tìm thấy idtower trong URL.");
        }
        const idContract = this.$route.params.idcontract;
        const DecodingContract = CryptoJS.enc.Utf8.stringify(CryptoJS.enc.Base64.parse(idContract));
        this.contractId = DecodingContract;
        if(DecodingContract ==0) {
            this.titlePage ="Thêm hợp đồng"
        } else {
            this.titlePage ="Sửa hợp đồng"
            this.getContract();
        }
        const roomid = store.getters['GetRoomId'];
        if(roomid) {
            this.IsRoom = true;
            this.contractData.roomId = roomid
        }
        this.GetAllServiceByTower();
        this.getRoombyTower();
    },
    methods:{
        //#region validate 
        addCustomer() {
            this.contractData.customers.push({
                id:0,
                fullName: "",
                doB: "",
                phoneNumber: "",
                email: null,
                cCCD: "",
                address: "",
                isRepresentative : false,
            });
        },
        addService(){
            this.contractData.services.push({
                serviceId: null,
                price: '',
                number: 1,

            })
        },
        closeService(index){
            this.contractData.services.splice(index, 1);
        },
        closeCustomer(index){
            this.contractData.customers.splice(index, 1);
        },
        goToFirstTab() {
            this.currentTab = this.tabs[0];
        },
        required (v) {
            return !!v || 'Vui lòng không để trống'
        },
        requiredNumber (v) {
            return !!v || 'Vui lòng nhập vào số'
        },
        formatDate(dateString) {
            const date = new Date(dateString);
            return date.toLocaleDateString('vi-VN'); // Định dạng: dd/MM/yyyy
        },
        //#endregion
        async getRoombyTower(){
            await apiClient.get(`/Room/GetRoomNoContract?towerID=${this.towerid}`)
                            .then(reponse =>{
                                this.selectRoom =reponse.data;
                            })
                            .catch(error =>{
                                this.message = "Lấy danh sách phòng bị lỗi " + error.response?.data?.message || error.message;
                                this.snackbar = true;
                                this.snackbarColor = 'red';
                            })
                            
        },
        async GetAllServiceByTower(){
            await apiClient.get(`/Service/GetAll?towerId=${this.towerid}`)
                            .then(reponse =>{
                                this.servicedata = reponse.data;
                            })
                            .catch(error =>{
                                this.message = "Lấy danh sách dịch vụ bị lỗi "+ error.response?.data?.message || error.message;
                                this.snackbar = true;
                                this.snackbarColor = 'red';
                            })
        },
        getContract(){
            apiClient.get(`/Contract/GetDetail?contractId=${this.contractId}`)
                    .then(reponse=>{
                        this.contractData = reponse.data;
                        this.handleRepresentativeChange();
                        for(let item of this.contractData.customers)
                        {
                            const dateTime = new Date(item.doB);
                            dateTime.setDate(dateTime.getDate() + 1);
                            item.doB = dateTime.toISOString().split('T')[0];
                        }
                    })
                    .catch(error =>{
                        this.message = "Lấy thông tin hợp đồng bị lỗi "+ error.response?.data?.message || error.message;
                        this.snackbar = true;
                        this.snackbarColor = 'red';
                    });
            
            
                   
        },
        onServiceSelected(service){
            const selectedService = this.servicedata.find(item => item.id === service.serviceId);
            if (selectedService) {
                service.price = selectedService.price;
                service.isOldNewNumber = selectedService.isOldNewNumber
                service.currentNumber = selectedService.currentNumber;
            } else {
                service.price = ''; // Xóa giá nếu không có dịch vụ
            }
        },
        CreateEditContract(){
            console.log(this.contractData);
            apiClient.post(`/Contract/CreateEdit`,this.contractData)
                    .then(()=>{
                        this.message = "Thao tác thành công"
                        this.snackbar = true;
                        this.snackbarColor = 'green';
                        this.$router.push({ name: 'contract'});
                    })
                    .catch(error =>{
                        this.message = "Đã xảy ra lỗi: "+ error.response?.data?.message || error.message;
                        this.snackbar = true;
                        this.snackbarColor = 'red';
                    })
        },
        handleRepresentativeChange() {
            // Kiểm tra xem có checkbox nào được chọn là người đại diện hay không
            this.isRepresentativeSelected = this.contractData.customers.some(customer => customer.isRepresentative);
        },
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
            this.contractData.customers[this.indexAddress].address = this.addressConfirm;
            this.viewDiaLogAddress = false
        }
    }
}
</script>

<template>
        <pageheader :title="titlePage" pageTitle="Hợp đồng" />
        <BRow>
            <div class="col-sm-12">
                <v-snackbar
                    v-model="snackbar"
                    :timeout="10000"
                    class="custom-snackbar"
                    :color="snackbarColor"
                >
                    <h5 class="text-center">{{ message }}</h5>
                </v-snackbar>
                <div id="basicwizard" class="form-wizard row justify-content-center">
                    <div class="col-sm-12 col-md-6 col-xxl-4 text-center">
                        <h3>{{ titlePage}}</h3>
                    </div>
                <div class="col-12">
                <div class="card">
                    <div class="card-body p-3">
                    <ul class="nav nav-pills nav-justified">
                        <li class="nav-item" data-target-form="#contactDetailForm">
                        <a href="#contactDetail" data-bs-toggle="tab" data-toggle="tab" class="nav-link active">
                            <i class="ph-duotone ph-user-circle"></i>
                            <span class="d-none d-sm-inline">Thông tin hợp đồng</span>
                        </a>
                        </li>
                        <!-- end nav item -->
                        <li class="nav-item" data-target-form="#customerDetailForm">
                        <a href="#customerDetail" data-bs-toggle="tab" data-toggle="tab" class="nav-link icon-btn">
                            <i class="ph-duotone ph-map-pin"></i>
                            <span class="d-none d-sm-inline">Thông tin khách hàng</span>
                        </a>
                        </li>
                        <!-- end nav item -->
                        <li class="nav-item" data-target-form="#serviceDetailForm">
                        <a href="#serviceDetail" data-bs-toggle="tab" data-toggle="tab" class="nav-link icon-btn">
                            <i class="ph-duotone ph-graduation-cap"></i>
                            <span class="d-none d-sm-inline">Thông tin dịch vụ</span>
                        </a>
                        </li>
                        <!-- end nav item -->
                    </ul>
                    </div>
                </div>
                <div class="card">
                    <div class="card-body">
                    <div class="tab-content">
                        <!-- BẮT ĐẦU: Xác định thanh tiến trình của bạn tại đây -->
                        <div id="bar" class="progress mb-3" style="height: 7px;">
                        <div class="bar progress-bar progress-bar-striped progress-bar-animated bg-success"></div>
                        </div>
                        <!-- END: Xác định thanh tiến trình của bạn ở đây -->
                        <!-- BẮT ĐẦU: Xác định các tab của bạn ở đây -->
                        <v-form v-model="form" ref="form"></v-form>
                        <div class="tab-pane show active" id="contactDetail">
                            <v-form v-model="form1" ref="form" id="contactForm" >
                                <div class="text-center">
                                    <h3 class="mb-2">Thông tin hợp đồng</h3>
                                    <small class="text-muted">Hãy thêm các thông tin của hợp đồng thuê phòng</small>
                                </div>
                                <div class="col mb-3 card">
                                    <div class="row card-body">
                                    <div class="col-sm-6" v-show="contractId == 0">
                                        <div class="form-group">
                                            <label class="form-label">Số phòng: </label>
                                            <v-select
                                                :clearable="IsRoom ==false"
                                                :items="selectRoom"
                                                item-title="name"
                                                item-value="id"
                                                variant="outlined"
                                                v-model="contractData.roomId"
                                                :readonly="IsRoom ==true"
                                                >
                                            </v-select>
                                        </div>
                                    </div>
                                    <div class="col-sm-4" v-show="contractId != 0">
                                        <div class="form-group">
                                            <label class="form-label">Số phòng: </label>
                                            <v-text-field variant="outlined"  v-model="contractData.roomName" readonly></v-text-field>
                                        </div>
                                    </div>
                                    <div class="col-sm-4" v-show="contractId != 0">
                                        <div class="form-group">
                                            <label class="form-label">Ngày bắt đầu: </label>
                                            <v-text-field variant="outlined" :value="formatDate(contractData.startDate)" v-model="contractData.startDate" readonly></v-text-field>
                                        </div>
                                    </div>
                                    <div class="col-sm-4" v-show="contractId != 0">
                                        <div class="form-group">
                                            <label class="form-label">Ngày kết thúc: </label>
                                            <v-text-field variant="outlined" :value="formatDate(contractData.endDate)"  v-model="contractData.endDate" readonly></v-text-field>
                                        </div>
                                    </div>
                                    <div class="col-sm-6" v-show="contractId == 0">
                                        <div class="form-group">
                                            <label class="form-label">Thời hạn hợp đồng(tháng): </label>
                                            <v-text-field variant="outlined" type="number" v-model="contractData.contractPeriod" clearable></v-text-field>
                                        </div>
                                    </div>
                                    <div class="col-sm-6">
                                        <div class="form-group">
                                            <label class="form-label">Tiền cọc: </label>
                                            <v-text-field variant="outlined" :rules="[requiredNumber]" type="number" v-model="contractData.deposit" clearable></v-text-field>
                                        </div>
                                    </div>
                                    <div class="col-sm-6">
                                        <div class="form-group">
                                            <label class="form-label">Ghi chú: </label>
                                            <v-text-field variant="outlined" clearable v-model="contractData.note"></v-text-field>
                                        </div>
                                    </div>
                                </div>
                                </div>
                            </v-form>
                        </div>
                        <div class="tab-pane" id="customerDetail">
                            <v-form v-model="form2" ref="form" id="customerForm">
                                <div class="text-center">
                                    <h3 class="mb-2">Thông tin của khách hàng</h3>
                                    <small class="text-muted">Hãy thêm các thông tin của các khách hàng thuê phòng</small>
                                </div>
                                <div v-for="(customer, index) in contractData.customers" :key="index" class="row mt-4">
                                    <div class="col card mb-3">
                                        <div class="card-header d-flex justify-content-between">
                                            <h4 class="card-title d-flex">
                                                <span>Thông tin của khách hàng {{ index + 1 }}</span>
                                                <v-checkbox label="Là người đại diện"
                                                    hide-details 
                                                    v-model="customer.isRepresentative"
                                                    :disabled="isRepresentativeSelected && !customer.isRepresentative"
                                                    @change="handleRepresentativeChange">
                                                </v-checkbox>
                                            </h4>
                                            
                                            <v-btn v-show="index >= 1" class="btn btn-primary" @click="closeCustomer(index)">Xoá</v-btn>
                                        </div>
                                        <div class="row card-body">
                                            <div class="col-sm-4">
                                                <div class="form-group">
                                                    <label class="form-label">Họ và tên: </label>
                                                    <v-text-field variant="outlined" :rules="[required]" clearable v-model="customer.fullName"></v-text-field>
                                                </div>
                                            </div>
                                            <div class="col-sm-4">
                                                <div class="form-group">
                                                    <label class="form-label">Ngày sinh: </label>
                                                    <input type="date" class="form-control" id="example-datemin" :rules="[required]" v-model="customer.doB" min="2000-01-02">
                                                </div>
                                            </div>
                                            <div class="col-sm-4">
                                                <div class="form-group">
                                                    <label class="form-label">Số điện thoại: </label>
                                                    <v-text-field variant="outlined" :rules="[required]" clearable v-model="customer.phoneNumber"></v-text-field>
                                                </div>
                                            </div>
                                            <div class="col-sm-4" v-if="customer.isRepresentative">
                                                <div class="form-group">
                                                    <label class="form-label">Email: </label>
                                                    <v-text-field variant="outlined" :rules="[index === 0 ? required : null]" clearable v-model="customer.email"></v-text-field>
                                                </div>
                                            </div>
                                            <div class="col-sm-4">
                                                <div class="form-group">
                                                    <label class="form-label">Số CCCD: </label>
                                                    <v-text-field variant="outlined" type="number" :rules=" [required]" clearable v-model="customer.cccd"></v-text-field>
                                                </div>
                                            </div>
                                            <div class="col-sm-4">
                                                <div class="form-group">
                                                    <label class="form-label">Địa chỉ: </label>
                                                    <v-text-field readonly variant="outlined" :rules="[required]" @click="(viewDiaLogAddress = !viewDiaLogAddress) && btnAddress(index)" v-model="customer.address"></v-text-field>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <v-btn class="btn btn-primary" @click="addCustomer">Thêm khách hàng</v-btn>
                            </v-form>
                        </div>
                        <!-- Kết thúc cửa số tab chi tiết khách hàng -->
                        <div class="tab-pane" id="serviceDetail">
                            <v-form v-model="form3" ref="form" id="serviceForm" method="post">
                                <div class="text-center">
                                    <h3 class="mb-2">Thông tin các dịch vụ</h3>
                                    <small class="text-muted">Hãy điền các thông tin của các dịch vụ áp dụng cho phòng</small>
                                </div>
                                <div class="card" v-for="(service, index) in contractData.services" :key="index">
                                    <div class="card-header d-flex justify-content-between">
                                        <h4 class="card-title">Thông tin của dịch vụ {{ index + 1 }} {{ service.isOldNewNumber == true ? "(Cần nhập vào số mới và cũ)" : "" }}</h4>
                                        <v-btn v-show="index >= 1" class="btn btn-primary" @click="closeService(index)">Đóng</v-btn>
                                    </div>
                                    <div class="row mt-4 card-body">
                                        <div class="col-sm-5">
                                            <div class="form-group">
                                            <label class="form-label">Tên dịch vụ: </label>
                                            <v-select
                                                clearable
                                                :items="servicedata"
                                                item-title="name"
                                                item-value="id"
                                                variant="outlined"
                                                v-model="service.serviceId"
                                                @update:model-value="onServiceSelected(service)"
                                                :rules="[required]">
                                            </v-select>
                                            </div>
                                        </div>
                                        <div class="col-sm-4">
                                            <div class="form-group">
                                            <label class="form-label">Giá: </label>
                                            <v-text-field variant="outlined" clearable :rules="[requiredNumber]" v-model="service.price" ></v-text-field>
                                            </div>
                                        </div>
                                        <div class="col-sm-3" v-if="!service.isOldNewNumber" v-show="service.serviceId">
                                            <div class="form-group">
                                            <label class="form-label">Số lượng người dùng: </label>
                                            <v-text-field type="number" variant="outlined" :rules="[requiredNumber]" v-model="service.number" clearable></v-text-field>
                                            </div>
                                        </div>
                                        <div class="col-sm-3" v-else v-show="service.serviceId">
                                            <div class="form-group">
                                            <label class="form-label">Số hiện tại: </label>
                                            <v-text-field type="number" variant="outlined" :rules="[requiredNumber]" v-model="service.currentNumber" clearable></v-text-field>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <v-btn class="btn btn-primary" @click="addService">Thêm dịch vụ</v-btn>
                            </v-form>
                        </div>
                        <!-- Kết thục cửa sổ tab chi tiết dịch vụ -->
                        <!-- START: Define your controller buttons here-->
                        <div class="d-flex wizard justify-content-between mt-3">
                            <!-- <div class="first">
                                <a  @click="goToFirstTab" class="btn btn-secondary">
                                Đầu tiên
                                </a>
                            </div>
                            <div class="d-flex">
                                <div class="previous me-2">
                                <a @click="goToPreviousTab" class="btn btn-secondary">
                                    Quay lại
                                </a>
                                </div>
                                <div class="next">
                                <a @click="goToNextTab()" class="btn btn-secondary mt-3 mt-md-0">
                                    Tiếp theo
                                </a>
                                </div>
                            </div> -->
                            <div class="last">
                                <BButton type="button" variant="primary" @click="CreateEditContract()" :disabled="(!form1 || !form2 || !form3)">Save Changes</BButton>
                            </div>
                        </div>
                        <!-- END: Define your controller buttons here-->
                    </div>
                    </div>
                </div>
                <!-- end tab content-->
                </div>
          </div>
        </div>
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
    </BRow>
</template>