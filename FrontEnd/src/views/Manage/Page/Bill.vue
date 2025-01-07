<style scoped>
    .table{
        text-align: center;
    }
</style>

<script>
    import pageheader from "@/components/page-header.vue"
    import apiClient from "@/plugins/axios";
    import Swal from "sweetalert2";
    import CryptoJS from 'crypto-js';
    import common from "@/components/common/JavaScripCommon"


export default {
    name: "PRODUCT-LIST",
    components: {
        pageheader
    },
    data(){
        return{
            viewdialog: false,
            titleDialog:'',
            form: false,
            headersTable:[
                    {title: 'STT', value: 'stt',sortable: true},
                    {title: 'Tên khách thuê',value:'customerName',sortable: true},
                    {title: 'Số phòng',value: 'numberOfRoom',sortable: true},
                    {title: 'Số tiền',value: 'totalAmount',sortable: true},
                    {title: 'Tháng',value: 'time',sortable: true},
                    {title: 'Ngày thanh toán',value: 'paymentDate',sortable: true},
                    {title: 'Trạng thái',value: 'status',sortable: true},
                    {title: 'Hành đồng',value: 'actions',sortable: false}
                ],
            billData: [
            ],
            selectBill: {
                id: 0,
                numberOfRoom:'',
                priceRoom: 0,
                addressTower: '',
                amount: 0,
                date: '',
                pathImgQRPay:'',
                stk: '',
                status: 0,
                service: [
                    {
                        id: 0,
                        isOldNewNumber: false,
                        name: '',
                        newNumber: null,
                        oldNumber: null,
                        unitPrice: 0,
                        usageNumber: 1,
                    }
                ],
                arises:[
                    {
                        id: 0,
                        amount: 0,
                        reason: '',
                    }
                ]
            },
            CalculateRoom:[
            ],
            searchCustomer: '',
            searchRoom: '',
            searchDate: '',
            searchStatus: null,
            viewdialogEdit:false,
            viewdialogCalculateRoomCharges: false,
            towerId: 0,
            message: '',
            snackbar: false,
            snackbarColor: '',
        }
    },
    created(){
        const idtower = this.$route.params.idtower;
        const DecodingIdTower = CryptoJS.enc.Utf8.stringify(CryptoJS.enc.Base64.parse(idtower));
        this.towerId = DecodingIdTower;
        this.getAllBill();
    },
    computed: {
        filteredBills() {
            return this.billData.filter((bill) => {
                const matchesCustomer = this.searchCustomer
                    ? bill.customerName?.toLowerCase().includes(this.searchCustomer.toLowerCase())
                    : true;

                const matchesRoom = this.searchRoom
                    ? bill.numberOfRoom?.toLowerCase().includes(this.searchRoom.toLowerCase())
                    : true;

                const matchesDate = this.searchDate
                    ? bill.time === this.searchDate.split('-').reverse().join('/')
                    : true;
                const matchesStatus = this.searchStatus
                    ? bill.status?.toLowerCase() === this.searchStatus.toLowerCase()
                    : true;

                return matchesCustomer && matchesRoom && matchesDate && matchesStatus;
            });
        },
    },
    methods:{
        deleteBill(id,name) {
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
                    text: `Bạn đang muốn xóa hoá đơn của phòng: ${name}`,
                    icon: "warning",
                    confirmButtonText: "Có!",
                    cancelButtonText: "Không!",
                    showCancelButton: true,
                })
                .then((confirm) => {
                    if (confirm.value) {
                            apiClient.delete(`/Bill/Delete/${id}`)
                                    .then(reponse=> {
                                        if(reponse.status){
                                            swalWithBootstrapButtons.fire(
                                            "Xóa thành công!",
                                            `Đã xóa thành công hoá đơn của phòng: ${name}`,
                                            "success")
                                            this.getAllBill();
                                        }
                                    })
                                    .catch(error =>{
                                        swalWithBootstrapButtons.fire(
                                            "Xóa không thành công",
                                            ` ${error.response?.data?.message || error.message}`,
                                            "error"
                                        );
                                    })
                    } else if ( /* Read more about handling dismissals below */ confirm.dismiss === Swal.DismissReason.cancel) return
                });
            },
        required (v) {
            return !!v || 'Vui lòng không để trống'
        },
        DetailBill(idBill){
            apiClient.get(`/Bill/GetDetail?billId=${idBill}`)
                    .then(response=>{
                        this.selectBill = response.data;
                    })
                    .catch(error=>{
                        this.message = "Lấy thông tin hoá đơn bị lỗi: " + error.response?.data?.message || error.message;
                        this.snackbar = true;
                        this.snackbarColor = 'red';
                    })
        },
        getStatusClass(status) {
            switch (status) {
                case 'Đã thanh toán':
                    return 'badge bg-light-success ms-2'; // class dành cho trạng thái Active
                case 'Chưa thanh toán':
                    return 'badge bg-light-danger ms-2'; // class dành cho trạng thái Inactive
                default:
                    return 'badge bg-light-warning' ; // class mặc định
            }
        },
        formatDate(dateString) {
            if (!dateString) return '';
            const [month, year] = dateString.split('/'); // Tách MM/YYYY
            //if (!month || !year) return ''; // Xử lý nếu không đúng định dạng
            return `${year}-${month.padStart(2, '0')}`; // Trả về định dạng YYYY-MM
        },
        getAllBill(){
            apiClient.get(`/Bill/GetAll?towerId=${this.towerId}`)
                    .then(response=>{
                        this.billData = response.data;
                    })
                    .catch(error=>{
                        this.snackbar = true;
                        this.message = 'Lấy hoá đơn bị lỗi ' + error.response?.data?.message || error.message;
                        this.snackbarColor = 'red';
                    })
        },
        FormatTablePrice(price) {
            if(price)
            return common.formatTablePrice(price);
        },
        btnUpdate(idBill){
            apiClient.get(`/Bill/GetDetail?billId=${idBill}`)
                .then(response=>{
                    this.selectBill = response.data;
                })
                .catch(error=>{
                    this.message = "Lấy thông tin hoá đơn bị lỗi: " + error.response?.data?.message || error.message;
                    this.snackbar = true;
                    this.snackbarColor = 'red';
                })
        },
        saveUpdate(){
            this.selectBill.sumArises = this.selectBill.arises.reduce((sum,item) => sum + (item.amount || 0),0);
            apiClient.put(`/Bill/Update`,this.selectBill)
                    .then(()=>{
                            this.message = "Sửa hóa đơn thành công";
                            this.snackbar = true;
                            this.snackbarColor = 'green';
                            this.viewdialogEdit = false;
                            this.getAllBill();
                        })
                        .catch(error=>{
                            this.message = "Sửa hoá đơn bị lỗi: " + error.response?.data?.message || error;
                            this.snackbar = true;
                            this.snackbarColor = 'red';
                        })
        },
        btnCalculateRoom(){
            this.CalculateRoom = [];
            apiClient.get(`/Bill/CalculateRoom?towerId=${this.towerId}`)
                    .then(response=>{
                        this.CalculateRoom = response.data;
                    })
                    .catch(error=>{
                        this.message = "Đã xảy ra lỗi: " + error.response?.data?.message || error;
                        this.snackbar = true;
                        this.snackbarColor = 'red';
                    })
        },
        SendInvoice(){
            apiClient.post(`/Bill/SendInvoice`,this.CalculateRoom)
                    .then(()=>{
                            this.message = "Tính tiền cho các phòng thành công. Hóa đơn đã được gửi đến khách thuê. Bạn hãy thi thoảng vào trang web để theo dõi trạng thái thanh toán của các khách thuê";
                            this.snackbar = true;
                            this.snackbarColor = 'green';
                            this.viewdialogCalculateRoomCharges = false;
                            
                            this.getAllBill();
                        })
                        .catch(error=>{
                            this.message = "Tính tiền bị lỗi: " + error.response?.data?.message || error;
                            this.snackbar = true;
                            this.snackbarColor = 'red';
                        })
        }
    }
}
</script>

<template>
        <pageheader title="" pageTitle="Hoá đơn" />
        <BRow>
            <v-snackbar
                v-model="snackbar"
                :timeout="10000"
                class="custom-snackbar"
                :color="snackbarColor"
            >
                <h5 class="text-center">{{ message }}</h5>
            </v-snackbar>
            <BCol class="col-sm-12">
                <BCard>
                    <BCardBody class="p-0">
                        <BRow>
                            <BCol class="col-sm-3 col-6"><input type="month" class="form-control" id="example-datemin" v-model="searchDate" min="2000-01-02" placeholder="Chọn thời gian"></BCol>
                            <BCol class="col-sm-3 col-6"><v-select label="Trạng thái phí" :items="['Đã thanh toán','Chưa thanh toán','Chờ xác nhận thanh toán']" v-model="searchStatus" clearable hide-details></v-select></BCol>
                            <BCol class="col-sm-3 col-4"><v-text-field label="Số phòng" variant="outlined" v-model="searchRoom" clearable hide-details></v-text-field></BCol>
                            <BCol class="col-sm-3 col-4"><v-text-field label="Tên khách hàng" variant="outlined" v-model="searchCustomer" clearable hide-details></v-text-field></BCol>
                        </BRow>
                    </BCardBody>
                </BCard>
                <BCard no-body class="table-card p-sm-2">
                    <BCardBody>
                        <BRow class="text-end pb-3">
                            <Bcol class="col-sm-6"></Bcol>
                            <BCol class="col-sm-6 col-12"><v-btn @click="(viewdialogCalculateRoomCharges = !viewdialogCalculateRoomCharges) && (btnCalculateRoom())" color="blue-lighten-1" class="mt-2">Tính tiền phòng</v-btn></BCol>
                        </BRow>
                        <v-data-table 
                            :headers = "headersTable"
                            :items="filteredBills"
                            class="border-sm rounded-lg">
                            <template v-slot:[`item.stt`]="{ index }">
                                {{ index + 1 }}
                            </template>
                            <template v-slot:[`item.totalAmount`]="{ item }">
                                <!-- Hiển thị giá đã định dạng -->
                                {{ FormatTablePrice(item.totalAmount) }}
                            </template>
                            <template v-slot:[`item.status`]="{ item }">
                                <span :class="getStatusClass(item.status)">
                                    {{ item.status }}
                                </span>
                            </template>
                            <template v-slot:[`item.actions`]="{ item }">
                                <v-icon small @click="(viewdialog = !viewdialog) &&(DetailBill(item.id))">mdi-eye</v-icon>
                                <v-icon class="ml-lg-3" v-show="item.status === 'Chưa thanh toán'" small @click="(viewdialogEdit = !viewdialogEdit) && (btnUpdate(item.id)) " >mdi-pencil-circle </v-icon>
                                <v-icon class="ml-lg-3"  v-show="item.status === 'Chưa thanh toán'" small @click="deleteBill(item.id,item.numberOfRoom)" >mdi-delete-empty </v-icon>
                            </template>
                        </v-data-table>
                    </BCardBody>
                </BCard>
                <BModal v-model="viewdialog" hide-footer :title="`Chi tiết hoá đơn phòng ${selectBill.numberOfRoom} tháng ${selectBill.date} `" modal-class="fadeInRight"
                    class="v-modal-custom" centered size="lg" >
                    <h6>Tên khách hàng: {{ selectBill.customerName }}</h6>
                    <h6 v-show="selectBill.status == 4">Hoá đơn đã được thanh toán vào ngày: {{ selectBill.datePayment}}</h6>
                    <h6 v-show="selectBill.status == 3">Hoá đơn đã được thanh toán vào ngày: {{ selectBill.datePayment}} và đang chờ xác nhận thanh toán</h6>
                    <h6 v-show="selectBill.status == 2">Hoá đơn chưa được thanh toán</h6>
                    <h4>Chi tiết hoá đơn:</h4>
                    <table class="table Info" >
                        <thead>
                            <tr>
                                <th style="width: 40%;">Tên dịch vụ</th>
                                <th>Số tiền</th>
                                <th>Sử dụng</th>
                                <th>Thành tiền</th>
                            </tr>
                        </thead>
                        <tbody>
                            <tr>
                                <td>Tiền phòng</td>
                                <td>{{ FormatTablePrice(selectBill.priceRoom)}}</td>
                                <td>1</td>
                                <td>{{ FormatTablePrice(selectBill.priceRoom)}}</td>
                            </tr>
                            <tr v-for="(serviceItem, index) in selectBill.service" :key="index">
                                <td>
                                    <div>Tiền {{ serviceItem.name }}</div> 
                                    <div v-show="serviceItem.newNumber != null">(Số cũ: {{ serviceItem.oldNumber }} - Số mới: {{ serviceItem.newNumber }})</div>
                                </td>
                                <td>{{FormatTablePrice(serviceItem.unitPrice) }}</td>
                                <td>{{ serviceItem.usageNumber }}</td>
                                <td>{{FormatTablePrice(serviceItem.unitPrice * serviceItem.usageNumber) }}</td>
                            </tr>
                            <tr v-for="(arise, index) in selectBill.arises" :key="index" v-show="selectBill.arises.length > 0">
                                <td>Phát sinh({{ arise.reason }})</td>
                                <td>{{ arise.amount }}</td>
                                <td>1</td>
                                <td>{{ arise.amount }}</td>
                            </tr>
                            <tr>
                                <td colspan="3"><h4>Tổng tiền</h4></td>
                                <td><h4>{{FormatTablePrice(selectBill.amount) }}</h4></td>
                            </tr>
                        </tbody>
                    </table>
                </BModal>
                <BModal v-model="viewdialogEdit" hide-footer title="Sửa hoá đơn phòng 101" modal-class="fadeInRight"
                    class="v-modal-custom" centered size="lg">
                    <h6>Tên khách hàng: {{ selectBill.customerName }}</h6>
                    <h6>Tiền phòng: {{ FormatTablePrice(selectBill.priceRoom) }}</h6>
                    <div class="card-body">
                        <v-form v-model="form">
                            <div class="card" v-for="(Service,index) in selectBill.service " :key="index" >
                                <div class="card-header" v-if="Service.newNumber != null">
                                    <h4>Dịch vụ {{ Service.name }}</h4>
                                </div>
                                <div class="card-body "  v-if="Service.newNumber != null">
                                    <BRow>
                                        <BCol class="col-lg-6">
                                            <div class="form-group">
                                                <label class="form-label">Số cũ:</label>
                                                <v-text-field type="number" v-model="Service.oldNumber" :rules="[v => (v !== null && v !== undefined && v !== '') || 'Trường này là bắt buộc']" variant="outlined" clearable placeholder="Nhập vào số cũ" class="input-control"></v-text-field>
                                            </div>
                                        </BCol>
                                        <BCol class="col-lg-6">
                                            <div class="form-group">
                                                <label class="form-label">Số mới:</label>
                                                <v-text-field type="number" v-model="Service.newNumber" :rules="[required]" variant="outlined" clearable placeholder="Nhập vào số mới" class="input-control"></v-text-field>
                                            </div>
                                        </BCol>
                                    </BRow>
                                </div>
                                <BRow class="align-items-center" v-show="Service.newNumber == null">
                                    <BCol class="col-lg-4 text-center">
                                        <h5>Dịch vụ: {{ Service.name }}</h5>
                                    </BCol>
                                    <BCol class="col-lg-8">
                                        <v-text-field v-model="Service.usageNumber" :rules="[required]" variant="outlined" clearable placeholder="Nhập vào số sử dụng" class="input-control"></v-text-field>
                                    </BCol>
                                </BRow>
                            </div>
                           
                        </v-form>
                    </div>
                    <div class="modal-footer v-modal-footer">
                        <BButton type="button" variant="light" @click="viewdialog = false">Close
                        </BButton>
                        <BButton type="button" variant="primary" @click="saveUpdate()" :disabled="!form">Save Changes</BButton>
                    </div>
                </BModal>
                <BModal v-model="viewdialogCalculateRoomCharges" hide-footer title="Tính tiền phòng" modal-class="fadeInRight"
                    class="v-modal-custom" centered size="lg">
                    <div class="card-body">
                        <v-form v-model="form">
                            <div class="card" v-for="(room,index) in CalculateRoom " :key="index" >
                                <div class="card-header">
                                    <h4>Nhập thông tin cho phòng {{ room.numberOfRoom }}</h4>
                                </div>
                                <div class="card-body">
                                    <BRow>
                                        <BCol class="col-lg-6" v-for="(service,index) in room.services" :key="index">
                                            <div class="form-group" v-if="service.isOldNewNumber == true">
                                                <label class="form-label">Số {{ service.name}} (số cũ: {{ service.oldNumber}})</label>
                                                <v-text-field type="number" v-model="service.newNumber" :rules="[required]" variant="outlined" clearable placeholder="Nhập vào số mới" class="input-control"></v-text-field>
                                            </div>
                                        </BCol>
                                    </BRow>
                                </div>
                            </div>
                        </v-form>
                    </div>
                    <div class="modal-footer v-modal-footer">
                        <BButton type="button" variant="light" @click="viewdialogCalculateRoomCharges = false">Close
                        </BButton>
                        <BButton type="button" variant="primary" @click="SendInvoice()" :disabled="!form">Save Changes</BButton>
                    </div>
                </BModal>
            </BCol>
        </BRow>
</template>