<template>
    <v-row class="m0 filter">
        <v-col cols="3"><b class="text-h4">Danh sách phòng</b></v-col>
        <v-col cols="3">
            <v-select
                clearable
                label="Trạng thái phòng"
                :items="StatusRoom"
                item-title="title"
                item-value="value"
                variant="outlined">
            </v-select>
        </v-col>
        <v-col cols="3">
            <v-select
                clearable
                label="Trạng thái phí"
                :items="StatusFee"
                item-title="title"
                item-value="value"
                variant="outlined">
            </v-select>
        </v-col>
        <v-col cols="3">
            <v-text-field clearable label="Số phòng"></v-text-field>
        </v-col>
    </v-row>
    <div class="information">
        <v-row class="header m0 d-flex ml-3">
            <p>Còn trống: {{ RoomAvailable }} | Đã cho thuê: {{ RoomRented }} | Chưa thu phí: {{ RoomNoFee }}</p>
            <div>
                <v-btn class="mr-6 rounded-xl">Giảm Giá phòng</v-btn>
                <v-btn class="mr-6 rounded-xl">Khách thuê</v-btn>
                <v-btn class="mr-6 rounded-xl">Thêm phòng</v-btn>
            </div>
        </v-row>
        <v-row class="m0">
            <v-btn v-for="(item,index) in floor" :key="index" class="ml-10" @click="selectFloor =item">Tầng {{ item }}</v-btn>
        </v-row>
        <div class="d-flex flex-wrap">
            <v-card class="pa-4 itemRoom" color="peach" dark v-for="item in Roomfilter" :key="item.id">
                <v-row justify="space-between">
                    <v-icon>mdi-home</v-icon>
                    <span>{{ item.Name }}</span>
                    <v-spacer></v-spacer>
                    <v-icon>mdi-currency-usd</v-icon>
                    <span>{{ item.PriceRoom }}</span>
                </v-row>
                <v-row class="mt-4">
                    <v-col>
                        <v-icon>mdi-account-circle</v-icon>
                        {{ item.customer }}
                    </v-col>
                </v-row>
                <v-row justify="space-around" class="mt-4">
                    <v-btn icon size="small">
                        <v-icon>mdi-refresh</v-icon>
                    </v-btn>
                    <v-btn icon size="small">
                        <v-icon>mdi-phone</v-icon>
                    </v-btn>
                    <v-btn icon size="small">
                        <v-icon>mdi-delete</v-icon>
                    </v-btn>
                    <v-btn icon size="small">
                        <v-icon>mdi-pencil</v-icon>
                    </v-btn>
                    <v-btn icon size="small">
                        <v-icon>mdi-eye</v-icon>
                    </v-btn>
                </v-row>
            </v-card>
        </div>
    </div>
</template>
<script>
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
                RoomAvailable: 0, //Phòng còn trống
                RoomRented: 0, //Phòng đã cho thuê
                RoomNoFee: 0, //Phòng chưa trả phí
                selectFloor:1,
                RoomData:[
                    {id: 1,floor:1, Name: '101', PriceRoom: '3.000.000', customer:'Phạm Quang Hưng, Phạm Thị Minh Trang'},
                    {id: 2,floor:1, Name: '102', PriceRoom: '2.500.000', customer:'Phạm Quang Hưng, Phạm Thị Minh Trang'},
                    {id: 3,floor:2, Name: '201', PriceRoom: '3.000.000', customer:'Phạm Quang Hưng, Phạm Thị Minh Trang'},
                    {id: 4,floor:2, Name: '202', PriceRoom: '3.500.000', customer:'Phạm Quang Hưng, Phạm Thị Minh Trang'},
                    {id: 5,floor:3, Name: '301', PriceRoom: '3.000.000', customer:'Phạm Quang Hưng, Phạm Thị Minh Trang'},
                    {id: 6,floor:3, Name: '302', PriceRoom: '4.000.000', customer:'Phạm Quang Hưng, Phạm Thị Minh Trang'},
                    {id: 7,floor:4, Name: '401', PriceRoom: '3.000.000', customer:'Phạm Quang Hưng, Phạm Thị Minh Trang'},
                    {id: 8,floor:4, Name: '402', PriceRoom: '5.000.000', customer:'Phạm Quang Hưng, Phạm Thị Minh Trang'},
                ],
            }
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
    width: 20%;
    margin: 1%;
    border-radius: 30px;
}
</style>