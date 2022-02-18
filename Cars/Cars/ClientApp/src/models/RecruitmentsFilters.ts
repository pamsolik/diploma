
export interface RecruitmentsFilters {
  pageSize: number,
  pageIndex: number,
  searchString: string,
  technology: number,
  dateFrom: Date,
  dateTo: Date,
  sortOrder: number,
}

export function FiltersDefault(): RecruitmentsFilters {
  return {
    pageSize: 25,
    pageIndex: 0,
    searchString: "",
    technology: 0,
    dateFrom: null,
    dateTo: null,
    sortOrder: 0,
  }
}
