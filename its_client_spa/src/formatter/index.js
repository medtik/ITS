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
  }
}
