<template>
    <v-row class="towerheader p0">
        <v-col cols="4"><h1>Danh sách khu trọ</h1></v-col>
        <v-col cols="4"><v-text-field clearable label="Địa chỉ"></v-text-field></v-col>
        <v-col cols="4"><v-btn class="ml-4" @click="btnAdd()">Thêm nhà trọ</v-btn></v-col>
    </v-row>
    <div class="d-flex flex-wrap item">
            <v-card class="pa-4 itemTower" color="peach" dark v-for="item in towerData" :key="item.id">
                    <v-icon>mdi-map-marker</v-icon>
                    <span>{{ item.name }}</span>
                    <p class="mt-2">Số phòng: {{item.roomnumber}}</p>
                    <p class="mt-2">Đã cho thuê: {{ item.Rented }}</p>
                    <p  class="mt-2">Còn trống: {{ item.stillEmpty }}</p>
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
    export default {
        data(){
            return{
                towerData: [
                    {id: 1, name: 'Nhà 11 Cầu Diễn', address: 'Số 11 ngõ 91 Cầu Diễn Bắc Từ Liêm Hà Nội',roomnumber: 3,Rented: 2,stillEmpty: 1},
                    {id: 2, name: 'Nhà 11 Cầu Diễn', address: 'Số 11 ngõ 91 Cầu Diễn Bắc Từ Liêm Hà Nội',roomnumber: 3,Rented: 2,stillEmpty: 1},
                    {id: 3, name: 'Nhà 11 Cầu Diễn', address: 'Số 11 ngõ 91 Cầu Diễn Bắc Từ Liêm Hà Nội',roomnumber: 3,Rented: 2,stillEmpty: 1},
                    {id: 4, name: 'Nhà 11 Cầu Diễn', address: 'Số 11 ngõ 91 Cầu Diễn Bắc Từ Liêm Hà Nội',roomnumber: 3,Rented: 2,stillEmpty: 1},
                    {id: 5, name: 'Nhà 11 Cầu Diễn', address: 'Số 11 ngõ 91 Cầu Diễn Bắc Từ Liêm Hà Nội',roomnumber: 3,Rented: 2,stillEmpty: 1},
                    {id: 6, name: 'Nhà 11 Cầu Diễn', address: 'Số 11 ngõ 91 Cầu Diễn Bắc Từ Liêm Hà Nội',roomnumber: 3,Rented: 2,stillEmpty: 1},
                ],
                dialogEdit: false,
                titleDialog: ''
            }
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
                this.$router.push({ name: 'towerDetails', params: { idtower } });
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