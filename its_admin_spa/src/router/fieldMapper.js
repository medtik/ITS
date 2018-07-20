const map = [
  {server: 'pageIndex', client: 'page'},
  {server: 'pageSize', client: 'rowsPerPage'},
  {server: 'orderBy', client: 'sortBy'},
  {server: 'searchValue', client: 'search'},
  {server: 'totalElement', client: 'total'},
  {server: 'phoneNumber', client: 'phone'},
  {server: 'emailAddress', client: 'email'},
  {server: 'areaName', client: 'area'},
];


function toServerField (obj){

}

function toClientFields(obj){
  for (let item of map){
    if(obj)
  }
}

export default {
  toServerField,
  toClientFields,
}
