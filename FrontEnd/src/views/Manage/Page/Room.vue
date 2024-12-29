<script>
    import pageheader from "@/components/page-header.vue"
    import Swal from "sweetalert2";
    import CryptoJS from 'crypto-js';
    import apiClient from "@/plugins/axios";
    import common from "@/components/common/JavaScripCommon"

    export default {
        name: "ROOM",
        components: {
            pageheader
        },
        data(){
            return{
                StatusFee:[
                    {title:'Chưa thanh toán', value:false},
                    {title:'Đã thanh toán',value:true}
                ],
                StatusRoom:[
                    {title: 'Còn trống', value: false},
                    {title: 'Đã cho thuê', value: true}
                ],
                headersTable:[
                    {title: 'STT', value: 'stt',sortable: true},
                    {title: 'Số phòng',value:'numberOfRoom',sortable: true},
                    {title: 'Khách hàng',value:'customerName',sortable: true},
                    {title: 'Số người ở tối da',value: 'noPStaying',sortable: true},
                    {title: 'Giá',value: 'priceRoom',sortable: true},
                    {title: 'Hành đồng',value: 'actions',sortable: false}
                ],
                viewdialogDetail: false,
                titleDialog:'',
                viewdialogEdit: false,
                form: false,
                towerId: 0,
                RoomData: [],
                selectRoom: {},
                message: '',
                snackbar: false,
                snackbarColor: '',
                checkout: {},
                viewdialogCheckOut: false,
                viewdialogChangeRoom: false,
                contractData: [],
                towerData: [],
                changeRoom: {
                    services:[
                    {
                        serviceId: '',
                        price: '',
                        number: 1,
                    },
                ],
                },
                roomDataChange: [],
                searchStatusRoom: null,
                searchName: '',
                searchRoomName: '',
                servicedata: [],
            }
        },
        created(){
            const idtower = this.$route.params.idtower;
            const DecodingIdTower = CryptoJS.enc.Utf8.stringify(CryptoJS.enc.Base64.parse(idtower));
            this.towerId = DecodingIdTower;
            this.GetRoomByTowerId();
        },
        computed:{
            filteredRooms() {
                return this.RoomData.filter((room) => {
                    // Lọc theo trạng thái phí
                    // const matchesStatusFee = this.selectedStatusFee
                    // ? room.statusFee === this.selectedStatusFee
                    // : true;

                    // Lọc theo trạng thái phòng
                    const matchesStatusRoom = this.searchStatusRoom !== undefined && this.searchStatusRoom !== null
                    ? (this.searchStatusRoom === true && room.customerName && room.customerName.trim() !== "") || // Đã cho thuê
                        (this.searchStatusRoom === false && (!room.customerName || room.customerName.trim() === "")) // Còn trống
                    : true;

                    // Lọc theo tên khách hàng
                    const matchesSearchName = this.searchName
                    ? room.customerName?.toLowerCase().includes(this.searchName.toLowerCase())
                    : true;

                    // Lọc theo số phòng
                    const matchesSearchRoomName = this.searchRoomName
                    ? room.numberOfRoom?.toLowerCase().includes(this.searchRoomName.toLowerCase())
                    : true;

                    return matchesStatusRoom &&  matchesSearchName && matchesSearchRoomName;
                });
            },
        },
        methods:{
            async GetRoomByTowerId(){
                await apiClient.get(`Room/GetAllRoom/${this.towerId}`)
                    .then(reponse =>{
                        this.RoomData =reponse.data;
                    })
                    .catch(error =>{
                        this.message = "Lấy danh sách phòng bị lỗi " + error;
                        this.snackbar = true;
                        this.snackbarColor = 'red';
                    })
            },
            EditRoom(roomId,title){
                this.titleDialog = title;
                if(roomId ===0){
                    this.selectRoom = {};
                    this.form = false;
                }
                else{
                    apiClient.get(`Room/GetDetail?roomId=${roomId}`)
                            .then(reponse => {
                                this.selectRoom = reponse.data;
                            })
                            .catch(error=>{
                                this.viewdialogEdit = false;
                                this.snackbar = true;
                                this.message = 'Lấy thông tin của phòng bị lỗi ' + error;
                                this.snackbarColor = 'red';
                            })
                }
            },
            async DetailRoom(roomId){
                this.titleDialog = 'Xem chi tiết phòng';
                this.form = false;
                await apiClient.get(`Room/GetDetail?roomId=${roomId}`)
                            .then(reponse => {
                                this.selectRoom = reponse.data;
                            })
                            .catch(error=>{
                                this.viewdialogEdit = false;
                                this.snackbar = true;
                                this.message = 'Lấy thông tin của phòng bị lỗi ' + error;
                                this.snackbarColor = 'red';
                            })
                if(this.selectRoom.customerName != ''){
                    await apiClient.get(`Room/GetContractByRoomId?roomId=${roomId}`)
                    .then(reponse => {
                        this.contractData = reponse.data;
                    })
                    .catch(error=>{
                        this.viewdialogEdit = false;
                        this.snackbar = true;
                        this.message = 'Lấy thông tin hợp đồng của phòng bị lỗi ' + error;
                        this.snackbarColor = 'red';
                    })
                }
                

            },
            deleteRoom(id,name) {
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
                    text: `Bạn đang muốn xóa phòng: ${name}`,
                    icon: "warning",
                    confirmButtonText: "Có!",
                    cancelButtonText: "Không!",
                    showCancelButton: true,
                })
                .then((confirm) => {
                    if (confirm.value) {
                            apiClient.delete(`/Room/DeleteRoom/${id}`)
                                    .then(reponse=> {
                                        console.log(reponse)
                                        if(reponse.status){
                                            swalWithBootstrapButtons.fire(
                                            "Xóa thành công!",
                                            `Đã xóa thành công phòng: ${name}`,
                                            "success")
                                            this.GetRoomByTowerId();
                                        }
                                        else{
                                            swalWithBootstrapButtons.fire(
                                                "Xóa không thành công",
                                                `Đã xảy ra lỗi khi xóa phòng: ${name}`,
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
            },
            FormatTablePrice(price) {
                return common.formatTablePrice(price);
            },
            required (v) {
                return !!v || 'Vui lòng không để trống'
            },
            requiredNumber (v) {
                return !!v || 'Vui lòng nhập vào số'
            },
            FormatPrice(){
                this.selectRoom.priceRoom = common.formatPrice(this.selectRoom.priceRoom);
            },
            CheckOutRoom(){
                this.viewdialogDetail = false;
                this.form = true;
            },
            ChangeRoom(id){
                this.viewdialogDetail = false;
                this.form = false;
                this.changeRoom = {
                    roomIdOld : id,
                    services:[
                        {
                            serviceId: '',
                            price: '',
                            number: 1,
                        }
                    ],
                }
                apiClient.get(`/Tower/GetAllTowerByLandLordId/${1}`)
                        .then(response=>{
                            this.towerData = response.data;
                        })
                        .catch(error =>{
                            this.message = 'Đã xảy ra lỗi ' + error;
                            this.snackbarColor = 'red'
                            this.snackbar = true
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
            addService(){
                this.changeRoom.services.push({
                    serviceId: '',
                    price: '',
                    number: 1,
                })
            },
            closeService(index){
                this.changeRoom.services.splice(index, 1);
            },
            createEditRoom(){
                this.selectRoom.towerid = this.towerId;
                if(this.selectRoom.id === undefined){
                    this.message = 'Thêm thành công'
                    this.selectRoom.id = 0;
                }
                else{
                    this.message = 'Sửa thành công'
                }
                if(!/^\d+$/.test(this.selectRoom.priceRoom))
                this.selectRoom.priceRoom =  this.selectRoom.priceRoom.replace(/[^\d]/g, '');
                apiClient.post(`/Room/CreateEditRoom`,this.selectRoom)
                .then(response => {
                    if(response.status){
                        this.snackbarColor = 'green'
                        this.snackbar = true
                        this.viewdialogEdit = false
                        this.GetRoomByTowerId();
                    }
                    else{
                        this.message = 'Đã xảy ra lỗi '
                        this.snackbarColor = 'red'
                        this.snackbar = true
                    }
                })
                .catch(error =>{
                    this.message = 'Đã xảy ra lỗi ' + error
                    this.snackbarColor = 'red'
                    this.snackbar = true
                })
            },
            formatDate(dateString) {
                const date = new Date(dateString);
                return date.toLocaleDateString('vi-VN'); // Định dạng: dd/MM/yyyy
            },
            async filterChangeRooms(){
                await apiClient.get(`Room/GetRoomNoContract?towerID=${this.changeRoom.towerId}`)
                    .then(reponse =>{
                        this.roomDataChange =reponse.data;
                    })
                    .catch(error =>{
                        this.message = "Lấy danh sách phòng để đổi bị lỗi " + error;
                        this.snackbar = true;
                        this.snackbarColor = 'red';
                    })
                await apiClient.get(`/Service/GetAll?towerId=${this.changeRoom.towerId}`)
                .then(reponse =>{
                    this.servicedata = reponse.data;
                })
                .catch(error =>{
                    this.message = "Lấy danh sách dịch vụ bị lỗi "+ error;
                    this.snackbar = true;
                    this.snackbarColor = 'red';
                })
            },
            btnSaveChangeRoom(){
                const selectedDate = new Date(this.changeRoom.timesChange); // Ngày người dùng chọn
                const currentDate = new Date(); // Ngày hiện tại

                // Đặt thời gian của currentDate và selectedDate về 00:00:00 để chỉ so sánh ngày
                currentDate.setHours(0, 0, 0, 0);
                selectedDate.setHours(0, 0, 0, 0);

                if (selectedDate < currentDate) {
                    this.message = "Thời gian đổi đang nhỏ hơn ngày hiện tại. Bạn vui lòng chọn lại";
                    this.snackbar = true;
                    this.snackbarColor = 'red';
                } else {
                    apiClient.put(`Room/ChangeRoom`,this.changeRoom)
                        .then(response => {
                            if(response.status){
                                this.snackbarColor = 'green'
                                this.snackbar = true
                                this.message = 'ĐỔi phòng thành công',
                                this.viewdialogChangeRoom = false
                                this.GetRoomByTowerId();
                            }
                            else{
                                this.message = 'Đã xảy ra lỗi '
                                this.snackbarColor = 'red'
                                this.snackbar = true
                            }
                        })
                        .catch(error =>{
                            this.message = 'Đã xảy ra lỗi ' + error
                            this.snackbarColor = 'red'
                            this.snackbar = true
                        })
                }
            }
        }
}
</script>

<template>
        <pageheader title="" pageTitle="Phòng" />
        <BRow>
            <Bcol class="col-sm-12">
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
                            <BCol class="col-sm-3 col-6">            
                                <v-select
                                    clearable
                                    label="Trạng thái phí"
                                    :items="StatusFee"
                                    item-title="title"
                                    item-value="value"
                                    variant="outlined"
                                    hide-details>
                                </v-select>
                            </BCol>
                            <BCol class="col-sm-3 col-6">
                                <v-select
                                    clearable
                                    label="Trạng thái phòng"
                                    :items="StatusRoom"
                                    item-title="title"
                                    item-value="value"
                                    v-model="searchStatusRoom"
                                    variant="outlined"
                                    hide-details>
                                </v-select>
                            </BCol>
                            <Bcol class="col-sm-3 col-6"><v-text-field label="Tên khách hàng" variant="outlined" v-model="searchName" clearable hide-details></v-text-field></Bcol>
                            <BCol class="col-sm-3 col-6">
                                <v-text-field label="Số phòng" variant="outlined" v-model="searchRoomName" clearable hide-details></v-text-field>
                            </BCol>
                        </BRow>
                    </BCardBody>
                </BCard>
                <BCard no-body class="table-card p-sm-2">
                    <BCardBody>
                        <BRow class="text-end pb-3">
                            <BCol class="col-sm-9 col-6"><v-btn color="blue-lighten-1" class="mt-2" @click="(viewdialogEdit = !viewdialogEdit) && (EditRoom(0,'Thêm dịch vụ'))"> Thêm phòng</v-btn></BCol>
                            <BCol class="col-sm-3 col-6"><v-btn color="blue-lighten-1" class="mt-2"> Tính tiền Phòng</v-btn></BCol>
                        </BRow>
                        <v-data-table 
                        :headers = "headersTable"
                            :items="filteredRooms"
                            class="border-sm rounded-lg">
                            <template v-slot:[`item.stt`]="{ index }">
                                {{ index + 1 }}
                            </template>
                            <template v-slot:[`item.customerName`]="{ item }">
                                <div>
                                    <template v-if="item.customerName">
                                    {{ item.customerName }}
                                    </template>
                                    <template v-else>
                                    <v-btn color="primary" @click="addCustomer(item)">
                                        Thêm khách
                                    </v-btn>
                                    </template>
                                </div>
                            </template>
                            <template v-slot:[`item.priceRoom`]="{ item }">
                                <!-- Hiển thị giá đã định dạng -->
                                {{ FormatTablePrice(item.priceRoom) }}
                            </template>
                            <template v-slot:[`item.actions`]="{ item }">
                                <v-icon small @click="(viewdialogDetail = !viewdialogDetail)&& (DetailRoom(item.id))" title="Xem chi tiết">mdi-eye</v-icon>
                                <v-icon class="ml-lg-3" small @click="(viewdialogEdit = !viewdialogEdit) && (EditRoom(item.id,'Sửa dịch vụ'))" title="Sửa phòng" >mdi-pencil-circle </v-icon>
                                <v-icon class="ml-lg-3" v-show="!item.customerName" small @click="deleteRoom(item.id,item.numberOfRoom)" title="Xoá phòng" >mdi-delete-empty </v-icon>
                                <v-icon class="ml-lg-3" v-show="item.customerName" small @click="(viewdialogCheckOut = !viewdialogCheckOut) && (CheckOutRoom(item.id))" title="Trả phòng">mdi-refresh</v-icon>
                                <v-icon class="ml-lg-3" v-show="item.customerName" small @click="(viewdialogChangeRoom = !viewdialogChangeRoom) && (ChangeRoom(item.id))" title="Đổi phòng">mdi-swap-horizontal</v-icon>
                                <v-icon class="ml-lg-3" v-show="item.customerName" small @click="(viewdialogChangeRoom = !viewdialogChangeRoom) && (ChangeRoom(item.id))" title="Tìm khách mới   ">mdi-account-plus</v-icon>
                            </template>
                        </v-data-table>
                    </BCardBody>
                </BCard>
                <BModal v-model="viewdialogEdit" hide-footer :title="titleDialog" modal-class="fadeInRight"
                    class="v-modal-custom" centered size="lg" >
                    <div class="card-body">
                        <v-form v-model="form" ref="form">
                            <BRow>
                                <BCol class="col-lg-6">
                                    <div class="form-group">
                                        <label class="form-label">Số phòng:</label>
                                        <v-text-field v-model="selectRoom.numberOfRoom" :rules="[required]" variant="outlined" clearable placeholder="Nhập vào số phòng" class="input-control"></v-text-field>
                                    </div>
                                </BCol>
                                <BCol class="col-lg-6">
                                    <div class="form-group">
                                        <label class="form-label">Thiết bị:</label>
                                        <v-text-field v-model="selectRoom.equipment" :rules="[required]" type="text" variant="outlined" clearable placeholder="Nhập vào các thiết bị có trong phòng" class="input-control"></v-text-field>
                                    </div>
                                </BCol>
                                <BCol class="col-lg-6">
                                    <div class="form-group">
                                        <label class="form-label">Số người ở:</label>
                                        <v-text-field v-model="selectRoom.noPStaying" :rules="[requiredNumber]" type="number" variant="outlined" clearable placeholder="Nhập vào số người tối đa được ở" class="input-control"></v-text-field>
                                    </div>
                                </BCol>
                                <BCol class="col-lg-6">
                                    <div class="form-group">
                                        <label class="form-label">Giá:</label>
                                        <v-text-field v-model="selectRoom.priceRoom" :rules="[requiredNumber]" type="text" @input="FormatPrice" variant="outlined" clearable placeholder="Nhập vào giá phòng" class="input-control"></v-text-field>
                                    </div>
                                </BCol>
                                <BCol class="col-lg-6">
                                    <div class="form-group">
                                        <label class="form-label">Số điện(nếu có):</label>
                                        <v-text-field v-model="selectRoom.numberElectric"  type="number" variant="outlined" clearable placeholder="Nhập vào số người tối đa được ở" class="input-control"></v-text-field>
                                    </div>
                                </BCol>
                                <BCol class="col-lg-6">
                                    <div class="form-group">
                                        <label class="form-label">Số nước(nếu có):</label>
                                        <v-text-field v-model="selectRoom.numberCountries" type="number" variant="outlined" clearable placeholder="Nhập vào số người tối đa được ở" class="input-control"></v-text-field>
                                    </div>
                                </BCol>
                                <BCol class="col-lg-6">
                                    <div class="form-group">
                                        <label class="form-label">Ghi chú:</label>
                                        <v-text-field v-model="selectRoom.note" type="number" variant="outlined" clearable placeholder="Nhập vào số người tối đa được ở" class="input-control"></v-text-field>
                                    </div>
                                </BCol>
                            </BRow>
                        </v-form>
                    </div>
                    <div class="modal-footer v-modal-footer">
                        <BButton type="button" variant="light" @click="viewdialog = false">Close
                        </BButton>
                        <BButton type="button" variant="primary" @click="createEditRoom()" :disabled="!form">Save Changes</BButton>
                    </div>
                </BModal>
                <BModal v-model="viewdialogDetail" hide-footer :title="titleDialog" modal-class="fadeInRight"
                    class="v-modal-custom" centered size="xl" >
                    <div class="card-body">
                        <BTabs class="mb-3" content-class="mt-3">
                            <BTab title="Thông tin phòng" id="home" active>
                                <div class="card-body">
                                    <v-form v-model="form" ref="form">
                                        <BRow>
                                            <BCol class="col-lg-4">
                                                <div class="form-group">
                                                    <label class="form-label">Số phòng:</label>
                                                    <v-text-field v-model="selectRoom.numberOfRoom" :rules="[required]" variant="outlined" placeholder="Nhập vào số phòng" readonly class="input-control"></v-text-field>
                                                </div>
                                            </BCol>
                                            <BCol class="col-lg-4">
                                                <div class="form-group">
                                                    <label class="form-label">Thiết bị:</label>
                                                    <v-text-field v-model="selectRoom.equipment" :rules="[required]" type="text" variant="outlined" readonly placeholder="Nhập vào các thiết bị có trong phòng" class="input-control"></v-text-field>
                                                </div>
                                            </BCol>
                                            <BCol class="col-lg-4">
                                                <div class="form-group">
                                                    <label class="form-label">Số người ở:</label>
                                                    <v-text-field v-model="selectRoom.noPStaying" :rules="[requiredNumber]" type="number" variant="outlined" readonly placeholder="Nhập vào số người tối đa được ở" class="input-control"></v-text-field>
                                                </div>
                                            </BCol>
                                            <BCol class="col-lg-4">
                                                <div class="form-group">
                                                    <label class="form-label">Giá:</label>
                                                    <v-text-field v-model="selectRoom.priceRoom" :rules="[requiredNumber]" type="text" @input="FormatPrice" variant="outlined" readonly placeholder="Nhập vào giá phòng" class="input-control"></v-text-field>
                                                </div>
                                            </BCol>
                                            <BCol class="col-lg-4">
                                                <div class="form-group">
                                                    <label class="form-label">Số điện(nếu có):</label>
                                                    <v-text-field v-model="selectRoom.numberElectric"  type="number" variant="outlined" readonly placeholder="Nhập vào số người tối đa được ở" class="input-control"></v-text-field>
                                                </div>
                                            </BCol>
                                            <BCol class="col-lg-4">
                                                <div class="form-group">
                                                    <label class="form-label">Số nước(nếu có):</label>
                                                    <v-text-field v-model="selectRoom.numberCountries" type="number" variant="outlined" readonly placeholder="Nhập vào số người tối đa được ở" class="input-control"></v-text-field>
                                                </div>
                                            </BCol>
                                            <BCol class="col-lg-4">
                                                <div class="form-group">
                                                    <label class="form-label">Ghi chú:</label>
                                                    <v-text-field v-model="selectRoom.note" type="number" variant="outlined" readonly placeholder="Nhập vào số người tối đa được ở" class="input-control"></v-text-field>
                                                </div>
                                            </BCol>
                                        </BRow>
                                    </v-form>
                                </div>
                            </BTab>
                            <BTab title="Thông tin hợp đồng" v-if="selectRoom.customerName != ''">
                                <div class="card-body" >
                                    <BRow>
                                        <BCol class="col-lg-4">
                                            <div class="form-group">
                                                <label class="form-label">Ngày bắt đầu:</label>
                                                <v-text-field v-model="contractData.startDate" :value="formatDate(contractData.startDate)" variant="outlined" readonly class="input-control"></v-text-field>
                                            </div>
                                        </BCol>
                                        <BCol class="col-lg-4">
                                            <div class="form-group">
                                                <label class="form-label">Ngày kết thúc:</label>
                                                <v-text-field v-model="contractData.endDate" :value="formatDate(contractData.endDate)" type="text" variant="outlined" readonly class="input-control"></v-text-field>
                                            </div>
                                        </BCol>
                                        <BCol class="col-lg-4">
                                            <div class="form-group">
                                                <label class="form-label">Tiền cọc:</label>
                                                <v-text-field v-model="contractData.deposit"  variant="outlined" readonly class="input-control"></v-text-field>
                                            </div>
                                        </BCol>
                                        <BCol class="col-lg-12">
                                            <div class="form-group">
                                                <label class="form-label">Ghi chú:</label>
                                                <v-text-field v-model="contractData.note"   variant="outlined" readonly class="input-control"></v-text-field>
                                            </div>
                                        </BCol>
                                    </BRow>
                                </div>
                            </BTab>
                            <BTab title="Thông tin khách hàng" v-if="selectRoom.customerName != ''">
                                <div class="card" v-for="(customer,index) in contractData.customers " :key="index">
                                    <div class="card-header">
                                        <h4>Khách hàng {{ index + 1 }}</h4>
                                    </div>
                                    <div class="card-body" >
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
                                                    <v-text-field v-model="customer.doB" :value="formatDate(customer.doB)" type="text" variant="outlined" readonly class="input-control"></v-text-field>
                                                </div>
                                            </BCol>
                                            <BCol class="col-lg-4">
                                                <div class="form-group">
                                                    <label class="form-label">Số điện thoại:</label>
                                                    <v-text-field v-model="customer.phoneNumber"  variant="outlined" readonly class="input-control"></v-text-field>
                                                </div>
                                            </BCol>
                                            <BCol class="col-lg-4">
                                                <div class="form-group">
                                                    <label class="form-label">Email:</label>
                                                    <v-text-field v-model="customer.email"   variant="outlined" readonly class="input-control"></v-text-field>
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
                        </BTabs>
                    </div>
                </BModal>
                <BModal v-model="viewdialogCheckOut" hide-footer title="Trả phòng" modal-class="fadeInRight"
                    class="v-modal-custom" centered size="lg" >
                    <div class="card-body">
                        <v-form v-model="form" ref="form">
                            <BRow>
                                <BCol class="col-lg-6">
                                    <div class="form-group">
                                        <label class="form-label">Số điện(nếu có):</label>
                                        <v-text-field v-model="checkout.newNumberElectric"  type="number" variant="outlined" clearable placeholder="Nhập vào số phòng" class="input-control"></v-text-field>
                                    </div>
                                </BCol>
                                <BCol class="col-lg-6">
                                    <div class="form-group">
                                        <label class="form-label">Số nước(nếu có):</label>
                                        <v-text-field v-model="checkout.newNumberCountries"  type="number" variant="outlined" clearable placeholder="Nhập vào các thiết bị có trong phòng" class="input-control"></v-text-field>
                                    </div>
                                </BCol>
                                <BCol class="col-lg-6">
                                    <div class="form-group">
                                        <label class="form-label">Tiền phạt(nếu có):</label>
                                        <v-text-field v-model="checkout.moneyPunish" type="number" variant="outlined" clearable placeholder="Nhập vào số người tối đa được ở" class="input-control"></v-text-field>
                                    </div>
                                </BCol>
                                <BCol class="col-lg-6">
                                    <div class="form-group">
                                        <label class="form-label">Ghi chú(nếu có):</label>
                                        <v-text-field v-model="checkout.note" type="number" @input="FormatPrice" variant="outlined" clearable placeholder="Nhập vào giá phòng" class="input-control"></v-text-field>
                                    </div>
                                </BCol>
                            </BRow>
                        </v-form>
                    </div>
                    <div class="modal-footer v-modal-footer">
                        <BButton type="button" variant="light" @click="viewdialogCheckOut = false">Close
                        </BButton>
                        <BButton type="button" variant="primary" @click="CreateEditService()" :disabled="!form">Save Changes</BButton>
                    </div>
                </BModal>
                <BModal v-model="viewdialogChangeRoom" hide-footer title="Đổi phòng" modal-class="fadeInRight"
                    class="v-modal-custom" centered size="xl" >
                    <div class="card-body">
                        <v-form v-model="form" ref="form">
                            <BRow>
                                <BCol class="col-lg-12">
                                    <div class="form-group">
                                        <label class="form-label">Nhà trọ:</label>
                                        <v-select
                                            clearable
                                            :items="towerData"
                                            item-title="address"
                                            item-value="id"
                                            variant="outlined"
                                            v-model="changeRoom.towerId"
                                            @update:modelValue="filterChangeRooms">
                                        </v-select>
                                    </div>
                                </BCol>
                                <BCol class="col-lg-4">
                                    <div class="form-group">
                                        <label class="form-label">Phòng:</label>
                                        <v-select
                                            clearable
                                            :items="roomDataChange"
                                            :disabled="!changeRoom.towerId"
                                            item-title="name"
                                            item-value="id"
                                            variant="outlined"
                                            v-model="changeRoom.roomIdNew">
                                        </v-select>
                                    </div>
                                </BCol>
                                <BCol class="col-lg-4">
                                    <div class="form-group">
                                        <label class="form-label">Thơi gian đổi</label>
                                        <input type="date" class="form-control" id="example-datemin" :rules="[required]" min="2000-01-02" v-model="changeRoom.timesChange">
                                    </div>
                                </BCol>
                                <BCol class="col-lg-4">
                                    <div class="form-group">
                                        <label class="form-label">Thời hạn hợp đồng</label>
                                        <v-text-field v-model="changeRoom.contractPeriod" type="number" :rules="[required]" variant="outlined" clearable placeholder="Nhập vào số tháng thời hạn hợp đồng" class="input-control"></v-text-field>
                                    </div>
                                </BCol>
                                <div class="card" v-show="changeRoom.towerId" v-for="(service, index) in changeRoom.services" :key="index">
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
                                <v-btn v-show="changeRoom.towerId" class="btn btn-primary" @click="addService">Thêm dịch vụ</v-btn>
                            </BRow>
                        </v-form>
                    </div>
                    <div class="modal-footer v-modal-footer">
                        <BButton type="button" variant="light" @click="viewdialogChangeRoom = false">Close
                        </BButton>
                        <BButton type="button" variant="primary" @click="btnSaveChangeRoom()" :disabled="!form">Save Changes</BButton>
                    </div>
                </BModal>
            </Bcol>
        </BRow>
</template>
<style>
    
</style>