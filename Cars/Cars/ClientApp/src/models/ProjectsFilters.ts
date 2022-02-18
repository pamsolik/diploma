
export interface ProjectsFilters {
  pageSize: number,
  pageIndex: number,
  searchString: string,
  technology: number,
  dateFrom: Date,
  dateTo: Date,
}

export function FiltersDefault(): ProjectsFilters {
  return {
    pageSize: 50,
    pageIndex: 0,
    searchString: "",
    technology: null,
    dateFrom: null,
    dateTo: null,
  }
}
