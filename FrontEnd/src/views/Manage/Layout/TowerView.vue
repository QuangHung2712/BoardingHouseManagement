<template>
    <v-row class="towerheader p0">
        <v-col cols="4"><h1>Danh sách khu trọ</h1></v-col>
        <v-col cols="4"><v-text-field clearable label="Địa chỉ"></v-text-field></v-col>
        <v-col cols="4"><v-btn class="ml-4" @click="btnAdd()">Thêm nhà trọ</v-btn></v-col>
    </v-row>
    <div class="d-flex flex-wrap item">
            <v-card class="pa-4 itemTower" color="peach" dark v-for="item in towerData" :key="item.id">
                    <v-icon>mdi-map-marker</v-icon>
                    <span>{{ item.address }}</span>
                    <p class="mt-2">Số phòng: {{item.sumRoom}}</p>
                    <p class="mt-2">Đã cho thuê: {{ item.roomRented }}</p>
                    <p  class="mt-2">Còn trống: {{ item.roomStillEmpty }}</p>
                <v-row justify="space-around" class="mt-2">
                    <v-btn icon size="small">
                        <v-icon>mdi-delete</v-icon>
                    </v-btn>
                    <v-btn icon size="small" @click="btnEdit()">
                        <v-icon>mdi-pencil</v-icon>
                    </v-btn>
                    <v-btn @click="goToTowerDetails(item.id)" icon size="small">
                        <v-icon>mdi-eye</v-icon>
                    </v-btn>
                </v-row>
            </v-card>
        </div>
        <v-dialog v-model="dialogEdit" class="dialog">
            <v-card>
                <v-card-title>{{titleDialog}}</v-card-title>
                <v-card-actions class="justify-space-between">
                    <v-btn @click="dialogEdit=false">Hủy</v-btn>
                    <v-btn>Lưu</v-btn>
                </v-card-actions>
            </v-card>
        </v-dialog>
</template>
<script>
    import CryptoJS from 'crypto-js';
    import apiClient from '@/plugins/axiosConfig';
    export default {
        data(){
            return{
                towerData: [

                ],
                dialogEdit: false,
                titleDialog: ''
            }
        },
        created(){
            this.GetAllTower();
        },
        methods:{
            btnAdd(){
                this.dialogEdit=true,
                this.titleDialog='Thêm nhà trọ'
            },
            btnEdit(){
                this.dialogEdit=true,
                this.titleDialog='Sửa nhà trọ'
            },
            goToTowerDetails(idtower) {
                //Mã hóa Id của tower
                const encryptedId = CryptoJS.enc.Base64.stringify(CryptoJS.enc.Utf8.parse(idtower));
                this.$router.push({ name: 'towerDetails', params: { idtower: encryptedId } });
            },
            GetAllTower(){
                apiClient.get(`/Tower/GetAllTowerByLandlordId/${1}`)
                    .then(reponse =>{
                        this.towerData =reponse.data
                        console.log(this.towerData)
                    })
                    .catch(error => {
                        console.error('Lấy thông tin của tòa nhà bị lỗi: ',error);
                    })
            }
        }
    }
</script>
<style scoped>
.m0{
    margin: 0px;
}
.p0{
    padding: 0px;
}
.itemTower{
    margin: 1%;
    border-radius: 25px;
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