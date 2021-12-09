export class CustomSort {

  private sortOrder = 1;
  private collator = new Intl.Collator(undefined, {
    numeric: true,
    sensitivity: "base",
  })

  constructor() {
  }

  public resolve(path, obj = self, separator = '.') {
    let properties = Array.isArray(path) ? path : path.split(separator);
    return properties.reduce((prev, curr) => prev && prev[curr], obj);
  }

  public startSort(property: string, order, type = "") {
    if (order === "desc") {
      this.sortOrder = -1;
    }
    return (a, b) => {
      if (type === "date") {
        return this.sortData(new Date(a[property]), new Date(b[property]));
      } else {
        return this.collator.compare(this.resolve(property, a), this.resolve(property, b)) * this.sortOrder;
      }
    }
  }

  private sortData(a, b) {
    if (a < b) {
      return -1 * this.sortOrder;
    } else if (a > b) {
      return this.sortOrder;
    } else {
      return 0;
    }
  }
}
