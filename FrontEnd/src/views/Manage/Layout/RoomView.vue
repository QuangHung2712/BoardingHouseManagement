<template>
    <v-row class="m0 filter">
        <v-col cols="3" class="p0 px-3"><b class="text-h5">Danh sách phòng</b></v-col>
        <v-col cols="3" class="p0 px-3">
            <v-select
                clearable
                label="Trạng thái phòng"
                :items="StatusRoom"
                item-title="title"
                item-value="value"
                variant="outlined">
            </v-select>
        </v-col>
        <v-col cols="3" class="p0 px-3">
            <v-select
                clearable
                label="Trạng thái phí"
                :items="StatusFee"
                item-title="title"
                item-value="value"
                variant="outlined">
            </v-select>
        </v-col>
        <v-col cols="3" class="p0 px-3">
            <v-text-field clearable label="Số phòng"></v-text-field>
        </v-col>
    </v-row>
    <div class="information">
        <v-row class="header m0 d-flex ml-3">
            <p>Còn trống: {{ RoomAvailable }} | Đã cho thuê: {{ RoomRented }} | Chưa thu phí: {{ RoomNoFee }}</p>
            <div>
                <v-btn class="mr-6 rounded-xl">Giảm Giá phòng</v-btn>
                <v-btn class="mr-6 rounded-xl">Tính tiền phòng</v-btn>
                <v-btn class="mr-6 rounded-xl" @click="btnAddCreateRoom(0,'Thêm phòng')">Thêm phòng</v-btn>
            </div>
        </v-row>
        <!-- <v-tabs class="m0">
            <v-tab v-for="(item,index) in floor" :key="index" class="ml-10" @click="selectFloor =item">Tầng {{ item }}</v-tab>
        </v-tabs> -->
        <div class="d-flex flex-wrap item">
            <v-card class="pa-4 itemRoom" color="peach" dark v-for="item in RoomData" :key="item.id" :class="item.customer ===null ? 'itemRoomEmpty':'' ">
                <v-row justify="space-between pa-2">
                    <v-icon>mdi-home</v-icon>
                    <span>{{ item.numberOfRoom }}</span>
                    <v-spacer></v-spacer>
                    <v-icon>mdi-currency-usd</v-icon>
                    <span>{{ item.priceRoom }}</span>
                </v-row>
                <v-row class="mt-4">
                    <v-col>
                        <v-icon>mdi-account-circle</v-icon>
                        {{ item.CustomerName }}
                    </v-col>
                </v-row>
                <v-row class="justify-space-between">
                    <router-link class="link" v-show="item.customer==null">Thêm khách</router-link>
                    <v-btn v-show="item.customer!=null" @click="BtnTimKhachMoi(item.id,item.Name)">Tìm khách mới</v-btn>
                </v-row>
                <v-row justify="space-around" class="mt-4">
                    <v-btn v-show="item.customer !== null" icon size="small" @click="ViewdialogPayChange(item.id,'Trả Phòng')">
                        <v-icon>mdi-refresh</v-icon>
                    </v-btn>
                    <v-btn v-show="item.customer !== null" icon size="small" @click="ViewdialogPayChange(item.id,'Đổi phòng')">
                        <v-icon>mdi-phone</v-icon>
                    </v-btn>
                    <v-btn icon size="small" @click="btnDeleteRoom(item.id,item.numberOfRoom)">
                        <v-icon >mdi-delete</v-icon>
                    </v-btn>
                    <v-btn icon size="small" @click="btnAddCreateRoom(item.id,'Sửa phòng')">
                        <v-icon >mdi-pencil</v-icon>
                    </v-btn>
                    <v-btn icon size="small" @click="btnAddCreateRoom(item.id,'Xem phòng')">
                        <v-icon >mdi-eye</v-icon>
                    </v-btn>
                </v-row>
            </v-card>
        </div>
    </div>
    <v-dialog v-model="dialogEdit" class="dialog">
            <v-card>
                <v-card-title class="text-h4">{{titleDialog}}</v-card-title>
                <v-row class="m0 text-center">
                    <v-col cols="6" class="p0">
                        <v-row class="m0">
                            <v-col cols="4" >Số phòng</v-col>
                            <v-col cols="8"><v-text-field v-model="selectRoom.numberOfRoom" clearable></v-text-field></v-col>
                        </v-row>
                    </v-col>
                    <v-col cols="6" class="p0">
                        <v-row class="m0">
                            <v-col cols="4">Tầng</v-col>
                            <v-col cols="8"><v-text-field clearable></v-text-field></v-col>
                        </v-row>
                    </v-col>
                    <v-col cols="6" class="p0">
                        <v-row class="m0">
                            <v-col cols="4">Thiết bị</v-col>
                            <v-col cols="8"><v-text-field v-model="selectRoom.equipment" clearable></v-text-field></v-col>
                        </v-row>
                    </v-col>
                    <v-col cols="6" class="p0">
                        <v-row class="m0">
                            <v-col cols="4">Số người ở</v-col>
                            <v-col cols="8"><v-text-field clearable type="number" v-model="selectRoom.noPStaying" disabled ></v-text-field></v-col>
                        </v-row>
                    </v-col>
                    <v-col cols="6" class="p0">
                        <v-row class="m0">
                            <v-col cols="4">Giá Phòng</v-col>
                            <v-col cols="8">
                                <v-text-field 
                                clearable 
                                v-model="selectRoom.priceRoom" 
                                @input="formatCurrency" 
                                append-inner="VND">
                                </v-text-field>
                            </v-col>
                        </v-row>
                    </v-col>
                    <v-col cols="6" class="p0">
                        <v-row class="m0">
                            <v-col cols="4">Ghi chú</v-col>
                            <v-col cols="8"><v-textarea clearable v-model="selectRoom.note"></v-textarea></v-col>
                        </v-row>
                    </v-col>
                </v-row>
                <v-card-actions class="justify-space-between">
                    <v-btn @click="dialogEdit=false">Hủy</v-btn>
                    <v-btn >Lưu</v-btn>
                </v-card-actions>
            </v-card>
    </v-dialog>
    <v-dialog v-model="dialogPayChange" style="width: 40%;">
        <v-card>
            <v-card-title class="d-flex justify-space-between"> 
                <span class="pl-10">{{ titledialogPayChange }}</span>
                <v-spacer></v-spacer>
                <v-icon>mdi-home</v-icon>
                <span class="pr-10">{{ selectRoom.Name }}</span>
            </v-card-title>
            <div v-show="titledialogPayChange ==='Trả Phòng'" class="mx-3 bg-dialog">
                    <v-row class="m0 align-center px-5 py-1">
                        <v-col cols="3" class="p0">Số điện mới</v-col>
                        <v-col cols="9" class="p0 mt-1"><v-text-field type="number" clearable></v-text-field></v-col>
                    </v-row>
                    <v-row class="m0 align-center px-5 py-1">
                        <v-col cols="3" class="p0">Số nước mới</v-col>
                        <v-col cols="9" class="p0 mt-1"><v-text-field type="number" clearable></v-text-field></v-col>
                    </v-row>
                    <v-row class="m0 align-center px-5 py-1">
                        <v-col cols="3" class="p0">Tiền phạt</v-col>
                        <v-col cols="9" class="p0 mt-1"><v-text-field clearable></v-text-field></v-col>
                    </v-row>
                    <v-row class="m0 align-center px-5 py-1">
                        <v-col cols="3" class="p0">Ghi chú</v-col>
                        <v-col cols="9" class="p0 mt-1"><v-textarea rows="4" clearable></v-textarea></v-col>
                    </v-row>
            </div>
            <div v-show="titledialogPayChange ==='Đổi phòng'" class="mx-3 bg-dialog">
                    <v-row class="m0 align-center px-5 py-1">
                        <v-col cols="3" class="p0">Nhà trọ</v-col>
                        <v-col cols="9" class="p0 mt-1"><v-select clearable></v-select></v-col>
                    </v-row>
                    <v-row class="m0 align-center px-5 py-1">
                        <v-col cols="3" class="p0">Tầng</v-col>
                        <v-col cols="9" class="p0 mt-1"><v-select clearable></v-select></v-col>
                    </v-row>
                    <v-row class="m0 align-center px-5 py-1">
                        <v-col cols="3" class="p0">Phòng</v-col>
                        <v-col cols="9" class="p0 mt-1"><v-select clearable></v-select></v-col>
                    </v-row>
                    <v-row class="m0 align-center px-5 py-1">
                        <v-col cols="3" class="p0">Thời Gian đổi</v-col>
                        <v-col cols="9" class="p0 mt-1"><v-select clearable></v-select></v-col>
                    </v-row>
            </div>
            <v-card-actions class="justify-space-between">
                <v-btn @click="dialogPayChange=false">Hủy</v-btn>
                <v-btn >Đồng ý</v-btn>
            </v-card-actions>
        </v-card>
    </v-dialog>
</template>
<script>
    import CryptoJS from 'crypto-js';
    import apiClient from '@/plugins/axiosConfig';
    export default{
        data(){
            return{
                StatusRoom:[
                    {title: 'Còn trống', value: false},
                    {title: 'Đã cho thuê', value: true}
                ],
                StatusFee:[
                    {title:'Chưa thanh toán', value:false},
                    {title:'Đã thanh toán',value:true}
                ],
                dialogEdit : false,
                dialogPayChange : false,
                titledialogPayChange: '',
                titleDialogEdit: '',
                selectRoomId: 0,
                RoomAvailable: 0, //Phòng còn trống
                RoomRented: 0, //Phòng đã cho thuê
                RoomNoFee: 0, //Phòng chưa trả phí
                selectFloor:1,
                towerId : 0, // Id của khu trọ đã được chọn
                RoomData:[],
                selectRoom:{},

            }
        },
        created(){
            const idtower = this.$route.params.idtower;
            //Giải mã
            const DecodingIdTower = CryptoJS.enc.Utf8.stringify(CryptoJS.enc.Base64.parse(idtower));
            this.towerId = DecodingIdTower
            this.getAllRoom();
        },
        mounted() {
            
        },
        computed:{
            floor(){
                return Math.max(...this.RoomData.map(room=>room.floor));
            },
            Roomfilter(){
                return this.RoomData.filter(room =>{
                    const matches = room.floor == this.selectFloor;
                    return matches
                })
            }
        },
        methods:{
            async getAllRoom(){
                await apiClient.get(`/Room/GetAllRoom/${this.towerId}`)
                    .then(reponse =>{
                        this.RoomData = reponse.data
                        console.log(this.RoomData)
                    })
                    .catch(error => {
                        console.error('Lấy thông tin của tòa nhà bị lỗi: ',error);
                    })
            },
            btnAddCreateRoom(roomId,title){
                if(title ==="Thêm phòng"){
                    this.selectRoom = {};
                }
                apiClient.get(`/Room/GetDetail?roomId=${roomId}`)
                .then(reponse =>{
                    this.selectRoom = reponse.data
                    console.log(this.selectRoom)
                })
                .catch(error => {
                        console.error('Lấy thông tin của phòng nhà bị lỗi: ',error);
                })
                this.dialogEdit = true;
                this.titleDialog = title;
                //this.selectRoomId = roomId;
                
            },
            btnAddRoom(){
                this.titleDialog = 'Thêm phòng';
                this.dialogEdit = true;
            },
            ViewdialogPayChange(roomId,title){
                this.selectRoomId = roomId;
                this.titledialogPayChange = title;
                this.dialogPayChange = true;
            },
            btnDeleteRoom(idRoom,nameRoom){
                var result = confirm('Bạn có chắc chắn muốn xóa phòng: ' + nameRoom);
                if(result){
                    try{
                        const response =  apiClient.patch(`/Room/DeleteRoom/${idRoom}?towerID=${this.towerId}`)
                        console.log(response)
                        if(response.status == 200){
                            this.getAllRoom();
                            alert("Xóa thành công")
                        }
                        else{
                            alert("Xóa không thành công")
                        }
                    }
                    catch(error){
                        console.error('Lỗi khi xóa hợp đồng:', error);
                        alert('Xóa không thành công');
                    }
                }
                else{
                    alert('Xoá không thành công');
                }
            },
            formatCurrency() {
                let numberValue = this.selectRoom.priceRoom.replace(/[^\d]/g, '');
                if (numberValue) {
                    numberValue = new Intl.NumberFormat('vi-VN', {
                        style: 'currency',
                        currency: 'VND',
                    }).format(numberValue);
                }
                this.selectRoom.priceRoom = numberValue;
            },
            BtnTimKhachMoi(roomId,roomName){
                var result = confirm('Bạn có chắc chắn muốn tìm khách mới cho phòng: ' + roomName);
                if(result){
                    alert('Thành công');
                }
            }
        }
    }
</script>
<style scoped>
.m0{
    margin: 0;
}
.filter{
    width: 100%;
    height: 13%;
}
.information{
    width: 100%;
    background-color: #d9d9d9;
    border-radius: 20px;
    height: 88%;
}
.information .header{
    justify-content: space-between;
    padding-top: 10px;
}
.itemRoom{
    background-color: #85c1e9;
    width: 20%;
    margin: 1%;
    border-radius: 30px;
}
.link{
    text-decoration: none !important;
    border: 1px solid black;
    border-radius: 25px;
    padding: 5px;
    background-color: blue;
    color: white;
}
.item{
    margin-left: 1%;
    margin-right: 1%;
}
.dialog{
    width: 80%;
    height: 80vh;
}
.dialog .v-btn{
    background-color: blue;
    color: whitesmoke;
}
.itemRoomEmpty{
    background-color: white;
}
.bg-dialog{
    background-color: #d9d9d9;
    border-radius: 25px;
}
</style>