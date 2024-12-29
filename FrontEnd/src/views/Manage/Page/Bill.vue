<style scoped>
    .table{
        text-align: center;
    }
</style>

<script>
import pageheader from "@/components/page-header.vue"
import Swal from "sweetalert2";


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
                    {title: 'Tên khách thuê',value:'customer',sortable: true},
                    {title: 'Số phòng',value: 'numberName',sortable: true},
                    {title: 'Số tiền',value: 'UnitPrice',sortable: true},
                    {title: 'Tháng',value: 'month',sortable: true},
                    {title: 'Ngày thanh toán',value: 'paymentDate',sortable: true},
                    {title: 'Trạng thái',value: 'status',sortable: true},
                    {title: 'Hành đồng',value: 'actions',sortable: false}
                ],
            billData: [
                {id: 1,customer: 'Phạm Văn A',numberName: '101',UnitPrice: '3.800.000 VNĐ',month: '12/2024',paymentDate:'21/12/2024',status: 'Đã thanh toán'},
                {id: 2,customer: 'Phạm Văn B',numberName: '102',UnitPrice: '4.800.000 VNĐ',month: '11/2024',paymentDate:'20/12/2024',status: 'Chưa thanh toán'}

            ],
            selectBill: [
                { serviceName: "Điện", oldNumber: 10, newNumber: 30 },
                { serviceName: "Nước", oldNumber: 20, newNumber: 50 },
            ],
            CalculateRoom:[
                {roomName:'101',newElectricity: null,newCountries: null },
                {roomName:'102',newElectricity: null,newCountries: null },
            ],
            searchCustomer: '',
            searchRoom: '',
            searchDate: '',
            searchStatus: null,
            viewdialogEdit:false,
            viewdialogCalculateRoomCharges: false,
        }
    },
    computed: {
        filteredBills() {
            return this.billData.filter((bill) => {
                const matchesCustomer = this.searchCustomer
                    ? bill.customer?.toLowerCase().includes(this.searchCustomer.toLowerCase())
                    : true;

                const matchesRoom = this.searchRoom
                    ? bill.numberName?.toLowerCase().includes(this.searchRoom.toLowerCase())
                    : true;

                const matchesDate = this.searchDate
                    ? this.formatDate(bill.month) === this.searchDate
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
                .then((result) => {
                    if (result.value) {
                        swalWithBootstrapButtons.fire(
                            "Xóa thành công!",
                            `Đã xóa hoá đơn của phòng thành công: ${name}`,
                            "success"
                        );
                    } else if (
                        /* Read more about handling dismissals below */
                        result.dismiss === Swal.DismissReason.cancel
                    ) {
                        swalWithBootstrapButtons.fire(
                            "Xóa không thành công",
                            `Đã xảy ra lỗi khi xóa hoá đơn của phòng: ${name}`,
                            "error"
                        );
                    }
                });
        },
        required (v) {
            return !!v || 'Vui lòng không để trống'
        },
        DetailBill(){
        },
        getStatusClass(status) {
            switch (status) {
                case 'Đã thanh toán':
                    return 'badge bg-light-success ms-2'; // class dành cho trạng thái Active
                case 'Chưa thanh toán':
                    return 'badge bg-light-danger ms-2'; // class dành cho trạng thái Inactive
                default:
                    return 'badge bg-light-warning'; // class mặc định
            }
        },
        formatDate(dateString) {
            if (!dateString) return '';
            const [month, year] = dateString.split('/'); // Tách MM/YYYY
            //if (!month || !year) return ''; // Xử lý nếu không đúng định dạng
            console.log(`${year}-${month.padStart(2, '0')}`)
            return `${year}-${month.padStart(2, '0')}`; // Trả về định dạng YYYY-MM
        },
    }
}
</script>

<template>
        <pageheader title="" pageTitle="Hoá đơn" />
        <BRow>
            <BCol class="col-sm-12">
                <BCard>
                    <BCardBody class="p-0">
                        <BRow>
                            <BCol class="col-sm-3 col-6"><input type="month" class="form-control" id="example-datemin" v-model="searchDate" min="2000-01-02" placeholder="Chọn thời gian"></BCol>
                            <BCol class="col-sm-3 col-6"><v-select label="Trạng thái phí" :items="['Đã thanh toán','Chưa thanh toán']" v-model="searchStatus" clearable hide-details></v-select></BCol>
                            <BCol class="col-sm-3 col-4"><v-text-field label="Số phòng" variant="outlined" v-model="searchRoom" clearable hide-details></v-text-field></BCol>
                            <BCol class="col-sm-3 col-4"><v-text-field label="Tên khách hàng" variant="outlined" v-model="searchCustomer" clearable hide-details></v-text-field></BCol>
                        </BRow>
                    </BCardBody>
                </BCard>
                <BCard no-body class="table-card p-sm-2">
                    <BCardBody>
                        <BRow class="text-end pb-3">
                            <Bcol class="col-sm-6"></Bcol>
                            <BCol class="col-sm-6 col-12"><v-btn @click="(viewdialogCalculateRoomCharges = !viewdialogCalculateRoomCharges)" color="blue-lighten-1" class="mt-2">Tính tiền phòng</v-btn></BCol>
                        </BRow>
                        <v-data-table 
                            :headers = "headersTable"
                            :items="filteredBills"
                            class="border-sm rounded-lg">
                            <template v-slot:[`item.stt`]="{ index }">
                                {{ index + 1 }}
                            </template>
                            <template v-slot:[`item.status`]="{ item }">
                                <span :class="getStatusClass(item.status)">
                                    {{ item.status }}
                                </span>
                            </template>
                            <template v-slot:[`item.actions`]="{ item }">
                                <v-icon small @click="(viewdialog = !viewdialog)">mdi-eye</v-icon>
                                <v-icon class="ml-lg-3" small @click="(viewdialogEdit = !viewdialogEdit)" >mdi-pencil-circle </v-icon>
                                <v-icon class="ml-lg-3" small @click="deleteBill(item.id,item.numberName)" >mdi-delete-empty </v-icon>
                            </template>
                        </v-data-table>
                    </BCardBody>
                </BCard>
                <BModal v-model="viewdialog" hide-footer title="Chi tiết hoá đơn phòng 101" modal-class="fadeInRight"
                    class="v-modal-custom" centered size="lg" >
                    <h6>Tên khách hàng: Phạm Văn A</h6>
                    <h6>Hoá đơn đã được thanh toán vào ngày 21/12/2024</h6>
                    <h4>Chi tiết hoá đơn:</h4>
                    <table class="table Info">
                    <thead>
                        <tr>
                            <th>STT</th>
                            <th style="width: 40%;">Tên dịch vụ</th>
                            <th>Số tiền</th>
                            <th>Sử dụng</th>
                            <th>Thành tiền</th>
                        </tr>
                    </thead>
                    <tbody>
                            <tr>
                                <td>1</td>
                                <td>Tiền phòng</td>
                                <td>3.000.000 VNĐ</td>
                                <td>1</td>
                                <td>3.000.000 VNĐ</td>
                            </tr>
                            <tr>
                                <td>2</td>
                                <td>
                                    <div>Tiền điện</div> 
                                    <div>(Số cũ: 100 - Số mới: 1000)</div>
                                </td>
                                <td>3.000 VNĐ</td>
                                <td>1</td>
                                <td>3.000.000 VNĐ</td>
                            </tr>
                            <tr>
                                <td>3</td>
                                <td>Tiền nước</td>
                                <td>30.000 VNĐ</td>
                                <td>1</td>
                                <td>3.000.000 VNĐ</td>
                            </tr>
                            <tr>
                                <td>4</td>
                                <td>Tiền phòng</td>
                                <td>3.000.000 VNĐ</td>
                                <td>1</td>
                                <td>3.000.000 VNĐ</td>
                            </tr>
                            <tr>
                                <th colspan="4">Tổng</th>
                                <td>3.000.000 VNĐ</td>
                            </tr>
                    </tbody>
                </table>
                </BModal>
                <BModal v-model="viewdialogEdit" hide-footer title="Sửa hoá đơn phòng 101" modal-class="fadeInRight"
                    class="v-modal-custom" centered size="lg">
                    <div class="card-body">
                        <v-form v-model="form">
                            <div class="card" v-for="(Service,index) in selectBill " :key="index">
                                <div class="card-header">
                                    <h4>Dịch vụ {{ Service.serviceName }}</h4>
                                </div>
                                <div class="card-body" >
                                    <BRow>
                                        <BCol class="col-lg-6">
                                            <div class="form-group">
                                                <label class="form-label">Số cũ:</label>
                                                <v-text-field v-model="Service.oldNumber" :rules="[required]" variant="outlined" clearable placeholder="Nhập vào số cũ" class="input-control"></v-text-field>
                                            </div>
                                        </BCol>
                                        <BCol class="col-lg-6">
                                            <div class="form-group">
                                                <label class="form-label">Số mới:</label>
                                                <v-text-field v-model="Service.newNumber" :rules="[required]" variant="outlined" clearable placeholder="Nhập vào số mới" class="input-control"></v-text-field>
                                            </div>
                                        </BCol>
                                    </BRow>
                                </div>
                            </div>
                        </v-form>
                    </div>
                    <div class="modal-footer v-modal-footer">
                        <BButton type="button" variant="light" @click="viewdialog = false">Close
                        </BButton>
                        <BButton type="button" variant="primary" @click="CreateEditArise()" :disabled="!form">Save Changes</BButton>
                    </div>
                </BModal>
                <BModal v-model="viewdialogCalculateRoomCharges" hide-footer title="Tính tiền phòng" modal-class="fadeInRight"
                    class="v-modal-custom" centered size="lg">
                    <div class="card-body">
                        <v-form v-model="form">
                            <div class="card" v-for="(Calculate,index) in CalculateRoom " :key="index">
                                <div class="card-header">
                                    <h4>Phòng {{ Calculate.roomName }}</h4>
                                </div>
                                <div class="card-body" >
                                    <BRow>
                                        <BCol class="col-lg-6">
                                            <div class="form-group">
                                                <label class="form-label">Số điện mới:</label>
                                                <v-text-field v-model="Calculate.oldNumber" :rules="[required]" variant="outlined" clearable placeholder="Nhập vào số điện mới" class="input-control"></v-text-field>
                                            </div>
                                        </BCol>
                                        <BCol class="col-lg-6">
                                            <div class="form-group">
                                                <label class="form-label">Số nước mới:</label>
                                                <v-text-field v-model="Calculate.newNumber" :rules="[required]" variant="outlined" clearable placeholder="Nhập vào số nước mới" class="input-control"></v-text-field>
                                            </div>
                                        </BCol>
                                    </BRow>
                                </div>
                            </div>
                        </v-form>
                    </div>
                    <div class="modal-footer v-modal-footer">
                        <BButton type="button" variant="light" @click="viewdialog = false">Close
                        </BButton>
                        <BButton type="button" variant="primary" @click="CreateEditArise()" :disabled="!form">Save Changes</BButton>
                    </div>
                </BModal>
            </BCol>
        </BRow>
</template>