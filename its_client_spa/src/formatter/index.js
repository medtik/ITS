import moment from "moment";

export default {
  getAllResponse({meta, currentList}) {
    return {
      list: currentList,
      total: meta.totalElement,
    }
  },

  getAllRequest({pagination, search, sortByStr}) {
    if (!sortByStr) {
      sortByStr = pagination.sortBy;
    }
    const descStr = pagination.descending ? 'desc' : 'asc';
    const orderBy = `${sortByStr}_${descStr}`;
    return {
      orderBy,
      pageIndex: pagination.page,
      pageSize: pagination.rowsPerPage,
      searchValue: search
    };
  },

  getPlanDayText(planDay, startDate) {
    switch (planDay) {
      case "0":
      case 0:
        return "Chưa lên lịch";
      default:
        if (startDate) {
          return moment(startDate)
            .add(planDay - 1, "days")
            .format('DD/MM/YYYY');
        } else {
          return `Ngày ${planDay}`
        }
    }
  },

  getDaysObj(planDay, startDate) {
    return {
      planDayText: this.getPlanDayText(planDay, startDate),
      planDay: _.toNumber(planDay),
      key: `day_${planDay}`,
    }
  },

  getStatusText(status) {
    switch (status) {
      case 0:
        return "Đang chờ";
      case 1:
        return "Đã chấp nhận";
      case 2:
        return "Đã từ chối";
    }
  },

  getCategoryIcon(category){
    switch (category) {
      case 'Ăn uống':
        return 'fas fa-utensils';
      case 'Nơi ở':
        return 'fas fa-hotel';
      case 'Mua sắm':
        return 'fas fa-shopping-cart';
      case 'Giải trí':
        return 'fas fa-gamepad';
      case 'Địa điểm thăm quan':
        return 'fas fa-university';
      case 'Dịch vụ':
        return 'fas fa-gas-pump';
      case 'Tiền tệ':
        return 'fas fa-credit-card';
      case 'Trụ sở ban ngành':
        return 'far fa-building';
      case 'Trạm xăng':
        return 'fas fa-gas-pump';
    }
  }
}
