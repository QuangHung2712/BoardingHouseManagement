<style scoped>
    .row-red {
    background-color: #ffebee !important; /* Màu nền đỏ nhạt */
    color: #b71c1c !important; /* Màu chữ đỏ đậm */
    }
</style>
<script>
import pageheader from "@/components/page-header.vue"
import Swal from "sweetalert2";
import CryptoJS from 'crypto-js';
import apiClient from "@/plugins/axios";


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
                    {title: 'Tên khách hàng',value:'customerName',sortable: true},
                    {title: 'Số phòng',value: 'numberOfRoom',sortable: true},
                    {title: 'Ngày thuê',value: 'startDate',sortable: true},
                    {title: 'Ngày hết hạn',value: 'endDate',sortable: true},
                    {title: 'Số điện thoại',value: 'phoneCustomer',sortable: true},
                    {title: 'Tiền cọc',value: 'deposit',sortable: true},
                    {title: 'Hành đồng',value: 'actions',sortable: false}
                ],
            contractData: [

            ],
            selectContract : {},
            towerId:0,
            message: '',
            snackbar: false,
            snackbarColor: '',
            roomData:[],
            searchRoom: null,
            searchCustomerName: '',
            searchRentalDate: '',
            searchComelDate: '',
            viewdialogContractExtension: false,
            contractExtension: {},
            ViewdialogEditContractSample: false,
            contractSampleNew: null
        }
    },
    created(){
        const idtower = this.$route.params.idtower;
        const DecodingIdTower = CryptoJS.enc.Utf8.stringify(CryptoJS.enc.Base64.parse(idtower));
        this.towerId = DecodingIdTower;
        this.GetAllContractBytowerId();
        this.GetRoomDropDown();
    },
    computed:{
        filteredContracts(){
            return this.contractData.filter((contract) => {
                const matchesSearchRoomName = this.searchRoom
                    ? contract.numberOfRoom == this.searchRoom
                    : true;

                // Lọc theo tên khách hàng
                const matchesSearchCustomerName = this.searchCustomerName
                    ? contract.customerName?.toLowerCase().includes(this.searchCustomerName.toLowerCase())
                    : true;

                // Lọc theo ngày thuê
                const matchesSearchFromlDate = this.searchRentalDate
                    ? new Date(contract.startDate).toISOString().split('T')[0] >= this.searchRentalDate
                    : true;
                const matchesSearchComelDate = this.searchRentalDate
                ? new Date(contract.endDate).toISOString().split('T')[0] <= this.searchComelDate
                : true;
                // Kết hợp tất cả các điều kiện
                return matchesSearchRoomName && matchesSearchCustomerName && matchesSearchFromlDate && matchesSearchComelDate;

            });
        }
    },
    methods:{
        GetAllContractBytowerId(){
            apiClient.get(`Contract/GetAll/${this.towerId}`)
                .then(reponse =>{
                    this.contractData =reponse.data;
                })
                .catch(error =>{
                    this.message = "Lấy danh sách hợp đồng bị lỗi " + error.response?.data?.message || error.message;
                    this.snackbar = true;
                    this.snackbarColor = 'red';
                })
        },
        deleteContract(id,name) {
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
                    text: `Bạn đang muốn xóa hợp của phòng: ${name}`,
                    icon: "warning",
                    confirmButtonText: "Có!",
                    cancelButtonText: "Không!",
                    showCancelButton: true,
                })
                .then((confirm) => {
                    if (confirm.value) {
                            apiClient.delete(`/Contract/Delete/${id}`)
                                    .then(reponse=> {
                                        if(reponse.status){
                                            swalWithBootstrapButtons.fire(
                                            "Xóa thành công!",
                                            `Đã xóa thành công hợp đồng của phòng: ${name}`,
                                            "success")
                                            this.GetAllContractBytowerId();
                                        }
                                        else{
                                            swalWithBootstrapButtons.fire(
                                                "Xóa không thành công",
                                                `Đã xảy ra lỗi khi xóa hợp đồng của phòng: ${name}`,
                                                "error"
                                            );
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
        DetailService(id,title){
            this.titleDialog = title;
            apiClient.get(`/Contract/GetDetail?contractId=${id}`)
                    .then(response=>{
                        this.selectContract = response.data;
                    })
                    .catch(error=>{
                        this.message = "Lấy thông tin của hợp đồng bị lỗi " + error.response?.data?.message || error.message;
                        this.snackbar = true;
                        this.snackbarColor = 'red';
                    })
        },
        required (v) {
            return !!v || 'Vui lòng không để trống'
        },
        CreateEditContract(id){
            this.$store.commit('setRoom', null);
            const encryptedId = CryptoJS.enc.Base64.stringify(CryptoJS.enc.Utf8.parse(id));
            this.$router.push({ name: 'createEdit', params: { idcontract: encryptedId } });
        },
        formatDate(dateString) {
            const date = new Date(dateString);
            return date.toLocaleDateString('vi-VN');
        },
        async GetRoomDropDown(){
            await apiClient.get(`/Room/GetDropDown?towerId=${this.towerId}`)
                            .then(response=>{
                                this.roomData = response.data;
                            })
                            .catch(error=>{
                                this.message = "Lấy thông tin của hợp đồng bị lỗi " + error.response?.data?.message || error.message;
                                this.snackbar = true;
                                this.snackbarColor = 'red';
                            })
                            
        },
        ExportWordcontract(contractId){
            apiClient.post(`/Contract/ExportWord/${contractId}`, null, { responseType: "blob" })
            .then((response) => {
                const fileURL = window.URL.createObjectURL(new Blob([response.data]));

                const fileLink = document.createElement("a");
                fileLink.href = fileURL;
                fileLink.setAttribute("download", "contract.docx");
                document.body.appendChild(fileLink);

                fileLink.click();
                document.body.removeChild(fileLink);
            })
            .catch((error) => {
                this.message = "Tải hợp đồng bị lỗi: " + error.response?.data?.message || error.message;
                this.snackbar = true;
                this.snackbarColor = "red";
            });
        },
        ContractExtension(id,name){
            this.contractExtension.contractId = id;
            this.titleDialog = 'Gia hạn hợp cho phòng ' + name;
        },
        btnSaveContractExtension(){
            apiClient.put(`/Contract/Extend`,this.contractExtension)
                .then(response => {
                    if(response.status){
                        const swalWithBootstrapButtons = Swal.mixin({
                            customClass: {
                                confirmButton: "btn btn-success",
                                cancelButton: "btn btn-danger ml-2",
                            },
                            buttonsStyling: false,
                        });

                        swalWithBootstrapButtons
                            .fire({
                                title: "Bạn có muốn sửa thông tin của hợp đồng không?",
                                text: `Gia hạn hợp đồng thành công. Bạn có muốn sửa thông tin của hợp đồng`,
                                icon: "warning",
                                confirmButtonText: "Có!",
                                cancelButtonText: "Không!",
                                showCancelButton: true,
                            })
                            .then((confirm) => {
                                if (confirm.value) {
                                    this.CreateEditContract(this.contractExtension.contractId);
                                } else if ( /* Read more about handling dismissals below */ confirm.dismiss === Swal.DismissReason.cancel) 
                                {
                                    this.viewdialogContractExtension = false;
                                    this.message = 'Gia hạn hợp đồng thành công'
                                    this.snackbarColor = 'green'
                                    this.snackbar = true
                                    this.GetAllContractBytowerId();
                                }
                });
                    }
                    else{
                        this.message = 'Đã xảy ra lỗi '
                        this.snackbarColor = 'red'
                        this.snackbar = true
                    }
                })
                .catch(error =>{
                    this.message = 'Đã xảy ra lỗi ' + error.response?.data?.message || error.message
                    this.snackbarColor = 'red'
                    this.snackbar = true
                })
        },
        ViewSampleContract() {
            apiClient.post(`/Contract/GetContractSample?landlordId=${1}`, null, { responseType: "blob" })
            .then((response) => {
                const fileURL = window.URL.createObjectURL(new Blob([response.data]));

                const fileLink = document.createElement("a");
                fileLink.href = fileURL;
                fileLink.setAttribute("download", "contractSample.docx");
                document.body.appendChild(fileLink);

                fileLink.click();
                document.body.removeChild(fileLink);
            })
            .catch((error) => {
                this.message = "Tải hợp đồng mẫu bị lỗi: " + error.response?.data?.message || error.message;
                this.snackbar = true;
                this.snackbarColor = "red";
            });
        },
        onFileSelected(event){
            const file = event.target.files[0];
            if (!file) {
                alert("Vui lòng chọn một tệp!");
                return;
            }
            this.contractSampleNew = file;
        },
        SaveContractSample(){
            const formData = new FormData();
            formData.append("landlordId", 1);
            formData.append("file", this.contractSampleNew);
            apiClient.put(`/Contract/EditContractSample`,formData)
                    .then(()=>{
                        this.snackbarColor = 'green';
                        this.snackbar = true;
                        this.message = 'Thay đổi hợp đồng mẫu thành công!';
                        this.ViewdialogEditContractSample = false;
                    })
                    .catch(error=>{
                        this.message = `Đã xảy ra lỗi: ${error.response?.data?.message || error.message}`;
                        this.snackbarColor = 'red';
                        this.snackbar = true;
                    })
        },
        getItemClass(item){
            console.log(item);
        }
    }
}
</script>

<style scoped>

</style>

<template>
        <pageheader title="" pageTitle="Hợp đồng" />
        <BRow>
            <BCol class="col-sm-12">
                <v-snackbar
                    v-model="snackbar"
                    :timeout="10000"
                    class="custom-snackbar"
                    :color="snackbarColor"
                >
                    <h5 class="text-center">{{ message }}</h5>
                </v-snackbar>
                <BCard>
                    <BCardBody class="p-0">
                        <BRow>
                            <BCol class="col-sm-3 col-6 d-flex align-end">
                                <v-select 
                                    label="Số phòng" 
                                    :items="roomData" 
                                    item-title="name"
                                    item-value="name" 
                                    v-model="searchRoom"
                                    hide-details 
                                    clearable>
                                </v-select>
                            </BCol>
                            <BCol class="col-sm-3 col-6 d-flex align-end"><v-text-field label="Tên khách hàng" variant="outlined" clearable hide-details v-model="searchCustomerName"></v-text-field></BCol>
                            <BCol class="col-sm-3 col-6">
                                <div class="form-group m-0">
                                    <label class="form-label m-0">Ngày thuê:</label>
                                    <input type="date" class="form-control" id="example-datemin" v-model="searchRentalDate" min="2000-01-02" placeholder="Tìm kiếm từ ngày">
                                </div>
                            </BCol>
                            <BCol class="col-sm-3 col-6">
                                <div class="form-group m-0">
                                    <label class="form-label m-0">Ngày hết hạn:</label>
                                    <input type="date" class="form-control" id="example-datemin" v-model="searchComelDate" min="2000-01-02" placeholder="Tìm kiếm từ ngày">
                                </div>
                            </BCol>
                        </BRow>
                    </BCardBody>
                </BCard>
                <BCard no-body class="table-card p-sm-2">
                    <BCardBody>
                        <BRow class="text-end pb-3 ">
                            <BCol class="col-sm-8 col-6"><v-btn @click="(ViewdialogEditContractSample = !ViewdialogEditContractSample) & (EditSampleContract)" color="blue-lighten-1" class="mt-2">Chỉnh sửa hợp đồng mẫu </v-btn></BCol>
                            <BCol class="col-sm-2 col-6"><v-btn @click="ViewSampleContract" color="blue-lighten-1" class="mt-2">Xem hợp đồng mẫu </v-btn></BCol>
                            <BCol class="col-sm-2 col-6"><v-btn @click="CreateEditContract(0)" color="blue-lighten-1" class="mt-2"> Thêm hợp đồng </v-btn></BCol>
                        </BRow>
                        
                        <v-data-table 
                            :headers = "headersTable"
                            :items="filteredContracts"
                            class="border-sm rounded-lg "
                            :item-class="getItemClass"
                            >
                            <template v-slot:[`item.stt`]="{ index }">
                                {{ index + 1 }}
                            </template>
                            <template v-slot:[`item.startDate`]="{ item }">
                                {{ formatDate(item.startDate) }}
                            </template>
                            <template v-slot:[`item.endDate`]="{ item }">
                                {{ formatDate(item.endDate) }}
                            </template>
                            <template v-slot:[`item.actions`]="{ item }">
                                <v-icon small @click="(viewdialog = !viewdialog) && (DetailService(item.id,'Chi tiết hợp đồng'))" title="Xem hợp đồng">mdi-eye</v-icon>
                                <v-icon class="ml-lg-3" small @click="CreateEditContract(item.id)" title="Sửa hợp đồng" >mdi-pencil-circle </v-icon>
                                <v-icon class="ml-lg-3" small @click="deleteContract(item.id,item.numberOfRoom)" title="Xóa hợp đồng" >mdi-delete-empty </v-icon>
                                <v-icon class="ml-lg-3" small @click="ExportWordcontract(item.id)" title="Xuất hợp đồng ra file Word" >mdi-download </v-icon>
                                <v-icon class="ml-lg-3" small @click="(viewdialogContractExtension = !viewdialogContractExtension) && (ContractExtension(item.id,item.numberOfRoom))" title="Gia hạn hợp đồng" >mdi-autorenew </v-icon>
                            </template>
                        </v-data-table>
                    </BCardBody>
                </BCard>
                <BModal v-model="viewdialog" hide-footer :title="titleDialog" modal-class="fadeInRight"
                    class="v-modal-custom" centered size="lg" >
                    <div class="card-body">
                        <BTabs class="mb-3" content-class="mt-3">
                            <BTab title="Thông tin hợp đồng" id="profile">
                                <div class="card-body">
                                    <BRow>
                                        <BCol class="col-lg-6">
                                            <div class="form-group">
                                                <label class="form-label">Số phòng:</label>
                                                <v-text-field v-model="selectContract.roomName" variant="outlined" readonly class="input-control"></v-text-field>
                                            </div>
                                        </BCol>
                                        <BCol class="col-lg-6">
                                            <div class="form-group">
                                                <label class="form-label">Tiền cọc:</label>
                                                <v-text-field v-model="selectContract.deposit" variant="outlined" readonly class="input-control"></v-text-field>
                                            </div>
                                        </BCol>
                                        <BCol class="col-lg-6">
                                            <div class="form-group">
                                                <label class="form-label">Ngày bắt đầu:</label>
                                                <v-text-field v-model="selectContract.startDate" :value="formatDate(selectContract.startDate)" variant="outlined" readonly class="input-control"></v-text-field>
                                            </div>
                                        </BCol>
                                        <BCol class="col-lg-6">
                                            <div class="form-group">
                                                <label class="form-label">Ngày kết thúc:</label>
                                                <v-text-field v-model="selectContract.endDate" :value="formatDate(selectContract.endDate)" variant="outlined" readonly class="input-control"></v-text-field>
                                            </div>
                                        </BCol>
                                        <BCol class="col-lg-12">
                                            <div class="form-group">
                                                <label class="form-label">Ghi chú:</label>
                                                <v-text-field v-model="selectContract.note" variant="outlined" readonly class="input-control"></v-text-field>
                                            </div> 
                                        </BCol>
                                    </BRow>
                                </div>
                            </BTab>
                            <BTab title="Thông tin khách hàng">
                                <div class="card" v-for="(customer,index) in selectContract.customers" :key="index">
                                    <div class="card-header">
                                        <h4>Thông tin khách hàng {{ index + 1 }}</h4>
                                    </div>
                                    <div class="card-body">
                                        <BRow>
                                            <BCol class="col-lg-4">
                                                <div class="form-group">
                                                    <label class="form-label">Họ và tên:</label>
                                                    <v-text-field v-model="customer.fullName" variant="outlined" readonly class="input-control"></v-text-field>
                                                </div>
                                            </BCol>
                                            <BCol class="col-lg-4">
                                                <div class="form-group">
                                                    <label class="form-label">Ngày sinh:</label>
                                                    <v-text-field v-model="customer.doB" :value="formatDate(customer.doB)" variant="outlined" readonly class="input-control"></v-text-field>
                                                </div>
                                                </BCol>
                                            <BCol class="col-lg-4">
                                                <div class="form-group">
                                                    <label class="form-label">Số điện thoại:</label>
                                                    <v-text-field v-model="customer.phoneNumber" variant="outlined" readonly class="input-control"></v-text-field>                                       
                                                </div>
                                            </BCol>
                                            <BCol class="col-lg-4">
                                                <div class="form-group">
                                                    <label class="form-label">Email:</label>
                                                    <v-text-field v-model="customer.email" variant="outlined" readonly class="input-control"></v-text-field>                                                                              
                                                </div>
                                            </BCol>
                                            <BCol class="col-lg-4">
                                                <div class="form-group">
                                                    <label class="form-label">CCCD:</label>
                                                    <v-text-field v-model="customer.cccd" variant="outlined" readonly class="input-control"></v-text-field>                                                                              
                                                </div>
                                            </BCol>
                                            <BCol class="col-lg-4">
                                                <div class="form-group">
                                                    <label class="form-label">Địa chỉ:</label>
                                                    <v-text-field v-model="customer.address" variant="outlined" readonly class="input-control"></v-text-field>                                                                                                                    
                                                </div>
                                            </BCol>
                                        </BRow>
                                    </div>
                                </div>
                            </BTab>
                            <BTab title="Thông tin dịch vụ">
                                <div class="card" v-for="(service,index) in selectContract.services" :key="index">
                                    <div class="card-header">
                                        <h4>Dịch vụ {{ index  + 1 }}</h4>
                                    </div>
                                    <div class="card-body">
                                        <BRow>
                                            <BCol class="col-lg-4">
                                                <div class="form-group">
                                                    <label class="form-label">Tên dịch vụ:</label>  
                                                    <v-text-field v-model="service.serviceName" variant="outlined" readonly class="input-control"></v-text-field>
                                                </div>
                                            </BCol>
                                            <BCol class="col-lg-4">
                                                <div class="form-group">
                                                    <label class="form-label">Giá tiền:</label>
                                                    <v-text-field v-model="service.price" variant="outlined" readonly class="input-control"></v-text-field>
                                               </div>
                                            </BCol>
                                            <BCol class="col-lg-4">
                                                <div class="form-group">
                                                    <label class="form-label">Số lượng:</label>
                                                    <v-text-field v-model="service.number" variant="outlined" readonly class="input-control"></v-text-field>
                                               </div>
                                            </BCol>
                                        </BRow>
                                    </div>
                                </div>
                            </BTab>
                        </BTabs>
                    </div>
                </BModal>
                <BModal v-model="viewdialogContractExtension" hide-footer :title="titleDialog" modal-class="fadeInRight"
                    class="v-modal-custom" centered size="md" >
                    <div class="card-body">
                        <v-form v-model="form" ref="form">
                            <BRow>
                                <BCol class="col-lg-12">
                                    <div class="form-group">
                                        <label class="form-label">Số tháng gia hạn thêm:</label>
                                        <v-text-field v-model="contractExtension.extensionPeriod"  :rules="[required]" type="number" variant="outlined" clearable placeholder="Nhập vào số tháng bạn muốn gia hạn thêm" class="input-control"></v-text-field>
                                    </div>
                                </BCol>
                            </BRow>
                        </v-form>
                    </div>
                    <div class="modal-footer v-modal-footer">
                        <BButton type="button" variant="light" @click="viewdialogContractExtension = false">Close
                        </BButton>
                        <BButton type="button" variant="primary" @click="btnSaveContractExtension()" :disabled="!form">Save Changes</BButton>
                    </div>
                </BModal>
                <BModal v-model="ViewdialogEditContractSample" hide-footer title="Chỉnh sửa hợp đồng mãu" modal-class="fadeInRight"
                    class="v-modal-custom" centered size="md" >
                    <div class="card-body">
                        <v-form v-model="form" ref="form">
                            <BRow>
                                <BCol class="col-lg-12">
                                    <div class="form-group">
                                        <label class="form-label w-100">Hợp đồng mới:</label>
                                        <input  type="file"  @change="onFileSelected"  accept=".doc,.docx,application/msword,application/vnd.openxmlformats-officedocument.wordprocessingml.document" class="input-control" />
                                        <p style="color: red;">Lưu ý không sửa các giá trị trong dấu "{}"</p>
                                    </div>
                                </BCol>
                            </BRow>
                        </v-form>
                    </div>
                    <div class="modal-footer v-modal-footer">
                        <BButton type="button" variant="light" @click="ViewdialogEditContractSample = false">Close
                        </BButton>
                        <BButton type="button" variant="primary" @click="SaveContractSample()">Save Changes</BButton>
                    </div>
                </BModal>
            </BCol>
        </BRow>
</template>
