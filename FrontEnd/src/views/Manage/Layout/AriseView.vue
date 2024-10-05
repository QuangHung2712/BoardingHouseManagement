<template>
    <v-row class="">
        <v-col cols="2">
            <b class="text-h5">Phát sinh</b>
        </v-col>
        <v-col cols=3>
            <v-select 
                label="Số phòng"
                clearable>
            </v-select>
        </v-col>
        <v-col cols="3">
            <v-select 
                label="Thơi gian"
                clearable>
            </v-select>
        </v-col>
        <v-col cols="4" class="d-flex justify-space-between mt-2">
            <v-btn @click="viewDiaLogEdit('Thêm',0)">Thêm phát sinh</v-btn>
            <v-btn @click="viewDiaLogEdit('Chi tiết',0)">Xuất Excel</v-btn>
        </v-col>
    </v-row>
    <v-data-table
        :headers = "headersTable">
        <template v-slot:[`item.actions`]="{ item }">
          <v-icon small @click="viewDetails(item)">mdi-eye</v-icon>
        </template>
    </v-data-table>
    <v-dialog v-model="dialogEdit">
        <v-card>
            <v-card-title>
                <v-row>
                    <v-col cols="4">
                        <b class="text-h4">{{ titleDialog }} phát sinh</b>
                    </v-col>
                    <v-col cols="2" class="text-center" v-show="titleDialog !=='Thêm'">
                        <v-icon>mdi-home</v-icon>
                        <span>{{ selectAriseId }}</span>
                    </v-col>
                    <v-col cols="6"  v-show="titleDialog === 'Chi tiết'" class="text-right">
                        <v-btn class="px-10" @click="viewDiaLogEdit('Sửa')">Sửa</v-btn>
                        <v-btn class="px-10 ml-10">Xóa</v-btn>
                        <v-btn class="px-10 ml-10" @click="btnClose()">Đóng</v-btn>
                    </v-col>
                </v-row>
            </v-card-title>
            <v-card-actions class="justify-space-between" v-show="titleDialog!=='Chi tiết'">
                <v-btn @click="btnCancel()">Hủy</v-btn>
                <v-btn>Lưu</v-btn>
            </v-card-actions>
        </v-card>
    </v-dialog>
</template>
<script>
    export default{
        data(){
            return {
                dialogEdit: false,
                titleDialog: '',
                selectAriseId: 0,
                headersTable:[
                    {title: 'Số phòng',value:'RoomName'},
                    {title: 'Số Tiền',value: 'Amount'},
                    {title: 'Ngày tạo', value: 'CreationDate'},
                    {title: 'Lý do', value: 'Reason'},
                    {title: 'Hành đồng',value: 'actions',sortable: false}
                ]
            }
        },
        methods:{
            viewDiaLogEdit(title,idArise){
                this.titleDialog=title;
                this.dialogEdit=true;
                idArise
            },
            btnClose(){
                this.dialogEdit = false;
                this.titleDialog = '';
            },
            btnCancel(){
                if(this.titleDialog === 'Sửa'){
                    this.titleDialog = 'Chi tiết'
                    this.dialogEdit = true
                    return;
                }
                this.dialogEdit = false;
            },
        }
    }
</script>
<style scoped>
    .v-dialog{
        width: 85%;
    }
</style>