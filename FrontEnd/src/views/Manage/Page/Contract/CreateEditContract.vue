<script>
import pageheader from "@/components/page-header.vue"
import common from "@/components/common/JavaScripCommon"
import apiClient from "@/plugins/axios";
import CryptoJS from 'crypto-js';

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
            
            contractData: {
                id: 0,
                customers: [
                    {
                        id:0,
                        fullName: "",
                        doB: null,
                        phoneNumber: "",
                        email: "",
                        cCCD: "",
                        address: "",
                    },
                ],
                services:[
                    {
                        serviceId: '',
                        price: '',
                        number: 1,
                    },
                ],
            }

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
                email: "",
                cCCD: "",
                address: "",
            });
        },
        addService(){
            this.contractData.services.push({
                serviceId: '',
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
        FormatPrice(index){
            this.contractData.services[index].price = common.formatPrice(this.contractData.services[index].price);
        },
        FormatpriceDeposit(){
            this.contractData.deposit = common.formatPrice(this.contractData.deposit)
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
                                this.message = "Lấy danh sách phòng bị lỗi " + error;
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
                                this.message = "Lấy danh sách dịch vụ bị lỗi "+ error;
                                this.snackbar = true;
                                this.snackbarColor = 'red';
                            })
        },
        getContract(){
            apiClient.get(`/Contract/GetDetail?contractId=${this.contractId}`)
                    .then(reponse=>{
                        this.contractData = reponse.data;
                        console.log(reponse.data)
                    })
                    .catch(error =>{
                        this.message = "Lấy thông tin hợp đồng bị lỗi "+ error;
                        this.snackbar = true;
                        this.snackbarColor = 'red';
                    })
        },
        onServiceSelected(service){
            const selectedService = this.servicedata.find(item => item.id === service.serviceId);
            if (selectedService) {
                service.price = selectedService.price;
            } else {
                service.price = ''; // Xóa giá nếu không có dịch vụ
            }
        },
        CreateEditContract(){
            apiClient.post(`/Contract/CreateEdit`,this.contractData)
                    .then(()=>{
                        this.message = "Thêm thành công"
                        this.snackbar = true;
                        this.snackbarColor = 'green';
                    })
                    .catch(error =>{
                        this.message = "Thêm hợp đồng bị lỗi "+ error;
                        this.snackbar = true;
                        this.snackbarColor = 'red';
                    })
        },
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
                                                clearable
                                                :items="selectRoom"
                                                item-title="name"
                                                item-value="id"
                                                variant="outlined"
                                                v-model="contractData.roomId"
                                                :rules="[required]">
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
                                            <v-text-field variant="outlined" type="number" :rules="[requiredNumber]" v-model="contractData.contractPeriod" clearable></v-text-field>
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
                                            <h4 class="card-title">Thông tin của khách hàng {{ index + 1 }}</h4>
                                            <v-btn v-show="index >= 1" class="btn btn-primary" @click="closeCustomer(index)">Đóng</v-btn>
                                        </div>
                                        <div class="row card-body">
                                        <div class="col-sm-4">
                                            <div class="form-group">
                                                <label class="form-label">Họ và tên: </label>
                                                <v-text-field variant="outlined" :rules="[required]" clearable v-model="customer.fullName"></v-text-field>
                                            </div>
                                        </div>
                                        <div class="col-sm-4">
                                            <div class="form-group">T
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
                                        <div class="col-sm-4">
                                            <div class="form-group">
                                                <label class="form-label">Email: </label>
                                                <v-text-field variant="outlined" :rules="[required]" clearable v-model="customer.email"></v-text-field>
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
                                                <v-text-field variant="outlined" :rules="[required]" clearable v-model="customer.address"></v-text-field>
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
                                        <h4 class="card-title">Thông tin của dịch vụ {{ index + 1 }}</h4>
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
                                            <v-text-field variant="outlined" clearable @input="FormatPrice(index)" :rules="[requiredNumber]" v-model="service.price" ></v-text-field>
                                            </div>
                                        </div>
                                        <div class="col-sm-3">
                                            <div class="form-group">
                                            <label class="form-label">Số lượng người dùng: </label>
                                            <v-text-field type="number" variant="outlined" :rules="[requiredNumber]" v-model="service.number" clearable></v-text-field>
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
        </BRow>
</template>